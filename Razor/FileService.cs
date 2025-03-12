namespace Razor
{
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class FileService
{
        //private readonly string _directoryPath = "\\nihonsaba.intern\\NS-Archiv\\Bücher";
        //private readonly string _directoryPath = "Z:\\"; // Use double backslashes for path
        private readonly string _directoryPath = @"\\WIN-QQ32S3B1B3S\t\NS-Archiv\Bücher";
        public List<string> GetFiles()
        {
        if (!Directory.Exists(_directoryPath))
        {
            return new List<string>(); 
        }
        return Directory.GetFiles(_directoryPath)
                        .Select(Path.GetFileName)
                        .ToList();
    }
}
}


