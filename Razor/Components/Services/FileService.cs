namespace Razor.Components.Services
{
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class FileService
{

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
        }

        public List<string> GetFolders(string? directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                return new List<string>();

            if (!Directory.Exists(directoryPath))
                return new List<string>();

            return Directory.GetDirectories(directoryPath)
                            .Select(Path.GetFileName)
                            .Where(name => name != null)
                            .ToList()!;
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


        public List<string> FetchFoldersFromFolder(string? file, string? directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                return new List<string>();

            var requestedPath = Path.Combine(directoryPath, file ?? string.Empty);

            if (!Directory.Exists(requestedPath))
                return new List<string>();

            return Directory.GetDirectories(requestedPath)
                            .Select(Path.GetFileName)
                            .Where(name => name != null)
                            .ToList()!;
        }


        public async Task<string?> GetTextFileContent(string dataSource, string path, string filename)
        {
            var fullPath = Path.Combine(dataSource, path, filename);
            if (File.Exists(fullPath))
            {
                return await File.ReadAllTextAsync(fullPath);
            }
            return null;
        }
        public async Task SaveTextFileContent(string dataSource, string path, string filename, string content)
        {
            var fullPath = Path.Combine(dataSource, path, filename);
            await File.WriteAllTextAsync(fullPath, content);
        }

        public void CreateFolder(string path, string folderName)
        {
            var fullPath = Path.Combine(path, folderName);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
        }

        public async Task CreateTextFile(string path, string fileName, string content)
        {
            var fullPath = Path.Combine(path, fileName);
            if (!File.Exists(fullPath))
                await File.WriteAllTextAsync(fullPath, content);
        }

        public void DeleteFile(string dataSource, string path, string filename)
        {
            var fullPath = Path.Combine(dataSource, path, filename);

            try { 
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);                
                } else if(Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, recursive: true);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen {fullPath}: {ex.Message}");
                throw;
            }

        }


    }

    public class FileData
    {
        public required string Name { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
    }
}


