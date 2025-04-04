namespace Razor.Components.Services
{
    using System.IO;

    public class RequestFileService
    {
        private readonly FileCleanupService _cleanupService;
        public RequestFileService(FileCleanupService cleanupService)
        {
            _cleanupService = cleanupService;
        }
        private readonly string _localFileDir = AppDomain.CurrentDomain.BaseDirectory + @"\req\";

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
   

        public async Task<Stream> GetStreamAsync(string requestedFile, string _FilePath)
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
    }
}