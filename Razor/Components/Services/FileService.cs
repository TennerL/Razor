namespace Razor.Components.Services
{
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class FileService
{

        //private string _directoryPath = @"\\WIN-QQ32S3B1B3S\t\";

        public List<string> GetFiles(string _directoryPath)
        {
            if(!Directory.Exists(_directoryPath))
            {
                return new List<string>();
            }
            var filesAndDirs = new List<string>();

            var files = Directory.GetFiles(_directoryPath).Select(Path.GetFileName).ToList();
            filesAndDirs.AddRange(files);

            return filesAndDirs;
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

        public List<string> FetchFilesFromFolder(string file, string _directoryPath)
        {
            var requestedPath = string.Concat(_directoryPath, file);
            if(!Directory.Exists(requestedPath))
            {
                return new List<string>();
            }
            var filesAndDirs = new List<string>();

            var files = Directory.GetFiles(requestedPath).Select(Path.GetFileName).ToList();
            filesAndDirs.AddRange(files);

            return filesAndDirs;
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
}


