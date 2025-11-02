namespace Razor.Components.Services
{
    using System.IO;
    using System.IO.Pipes;
    using System.Runtime.Intrinsics.Arm;
    using System.Security.Cryptography;
    using System.Text;

    public class RequestFileService
    {
        private readonly FileCleanupService _cleanupService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestFileService(FileCleanupService cleanupService, IHttpContextAccessor httpContextAccessor)
        {
            _cleanupService = cleanupService;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly string _localFileDir = AppDomain.CurrentDomain.BaseDirectory + @"\req\";
        private readonly string _shareDir = AppDomain.CurrentDomain.BaseDirectory + @"\share\";

        public async Task GetFile(string RequestedFile, string _FilePath)
        {
            var destinationFile = "";
            int lastIndex = RequestedFile.LastIndexOf("|");
            if (lastIndex != -1)
            {
                destinationFile = RequestedFile.Substring(lastIndex + 1);
                RequestedFile = RequestedFile.Replace("|", @"\");
            }
            else
            {
                destinationFile = RequestedFile;
            }
            var FullPathOrigin = string.Concat(_FilePath, RequestedFile);
            var FullPathDestination = string.Concat(_localFileDir, destinationFile);

            await using var sourceStream = File.OpenRead(FullPathOrigin);
            await using var destinationStream = File.Create(FullPathDestination);
            await sourceStream.CopyToAsync(destinationStream);

            _cleanupService.ScheduleFileDeletion(FullPathDestination, TimeSpan.FromMinutes(1));
        }
        public async Task<string> ShareFile(string requestedFileUrl)
        {
            if (string.IsNullOrWhiteSpace(requestedFileUrl))
                throw new ArgumentException("File URL cannot be empty", nameof(requestedFileUrl));

            requestedFileUrl = requestedFileUrl.Replace("|", @"\").Replace("/", @"\").Trim();

            string fileName = Path.GetFileName(requestedFileUrl);

            if (!File.Exists(requestedFileUrl))
                throw new FileNotFoundException("File not found", requestedFileUrl);

            string hash = GetHashString(requestedFileUrl);
            string pathDestination = Path.Combine(_shareDir, hash);
            Directory.CreateDirectory(pathDestination);

            string fullPathDestination = Path.Combine(pathDestination, fileName);

            try
            {
                if (File.Exists(fullPathDestination))
                    File.Delete(fullPathDestination);

                const int bufferSize = 1024 * 1024; 

                await using var sourceStream = new FileStream(
                    requestedFileUrl,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize,
                    useAsync: true);

                await using var destinationStream = new FileStream(
                    fullPathDestination,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize,
                    useAsync: true);

                await sourceStream.CopyToAsync(destinationStream, bufferSize);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to share file '{fileName}': {ex.Message}", ex);
            }

            string relativeUrl = $"{hash}/{fileName}";
            string baseUrl = string.Empty;

            if (_httpContextAccessor?.HttpContext?.Request != null)
            {
                var req = _httpContextAccessor.HttpContext.Request;
                baseUrl = $"{req.Scheme}://{req.Host}{req.PathBase}";
            }

            string finalUrl = $"{baseUrl}/share/{relativeUrl}".Replace("\\", "/");

            return finalUrl;
        }




        public Stream GetStream(string requestedFile, string _FilePath)
        {
            int lastIndex = requestedFile.LastIndexOf("|");
            if (lastIndex != -1)
            {
                requestedFile = requestedFile.Replace("|", @"\");
            }

            var fullPathOrigin = Path.Combine(_FilePath, requestedFile);

            if (!File.Exists(fullPathOrigin))
            {
                throw new FileNotFoundException("File not found", fullPathOrigin);
            }
            return new FileStream(fullPathOrigin, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

    }


}