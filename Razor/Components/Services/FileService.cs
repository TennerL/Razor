namespace Razor.Components.Services
{
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class FileService
{

        //private string _directoryPath = @"\\WIN-QQ32S3B1B3S\t\";

        public List<FileData> GetFiles(string _directoryPath)
        {
            if(!Directory.Exists(_directoryPath))
            {
                return new List<FileData>();
            }

            return Directory.GetFiles(_directoryPath)
                .Select(filePath => new FileInfo(filePath))
                .Select(file => new FileData
                {
                    Name = file.Name,
                    Size = file.Length,
                    Date = file.LastWriteTime
                })
                .OrderByDescending(file => file.Date)
                .ToList();
            
            //var filesAndDirs = new List<string>();

            //var files = Directory.GetFiles(_directoryPath).Select(Path.GetFileName).ToList();
            //filesAndDirs.AddRange(files);

            //return filesAndDirs;
        }

        public List<string> GetFolders(string _directoryPath)
        {
            if (!Directory.Exists(_directoryPath))
            {
                return new List<string>();
            }

            var folders = new List<string>();

            var directorys = Directory.GetDirectories(_directoryPath).Select(Path.GetFileName).ToList();
            folders.AddRange(directorys);
            return folders;
        }

            public List<FileData> FetchFilesFromFolder(string folder, string directoryPath)
            {
                string requestedPath = Path.Combine(directoryPath, folder);
                if (!Directory.Exists(requestedPath))
                {
                    return new List<FileData>();
                }

                return Directory.GetFiles(requestedPath)
                    .Select(filePath => new FileInfo(filePath))
                    .Select(file => new FileData
                    {
                        Name = file.Name,
                        Size = file.Length,
                        Date = file.LastWriteTime
                    })
                    .OrderByDescending(file => file.Date) 
                    .ToList();
            }
  

        public List<string> FetchFoldersFromFolder(string file, string _directoryPath)
        {
            var requestedPath = string.Concat(_directoryPath, file);
            if (!Directory.Exists(requestedPath))
            {
                return new List<string>();
            }
            var folders = new List<string>();
            var directorys = Directory.GetDirectories(requestedPath).Select(Path.GetFileName).ToList();
            folders.AddRange(directorys);

            return folders;
        }

    }

    public class FileData
    {
        public required string Name { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
    }
}


