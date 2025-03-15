namespace Razor.Components.Services
{
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualBasic;
    using Microsoft.AspNetCore.Components;

 

    public class RequestFileService 
    {
        private readonly FileCleanupService _cleanupService;
        public RequestFileService(FileCleanupService cleanupService)
        {
            _cleanupService = cleanupService;
        }
        private readonly string _localFileDir = AppDomain.CurrentDomain.BaseDirectory + @"\req\";
        private readonly string _FilePath = @"\\WIN-QQ32S3B1B3S\t\";
      

        public async Task GetFile(string RequestedFile)
        {
            var destinationFile = "";
            int lastIndex = RequestedFile.LastIndexOf("&");
            if (lastIndex != -1)
            {
                destinationFile = RequestedFile.Substring(lastIndex + 1);
                RequestedFile = RequestedFile.Replace("&", @"\");
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
    }
}