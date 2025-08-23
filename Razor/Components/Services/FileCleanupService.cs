using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class FileCleanupService : BackgroundService
{
    private readonly ConcurrentDictionary<string, DateTime> _filesToDelete = new();

    public void ScheduleFileDeletion(string filePath, TimeSpan delay)
    {
        _filesToDelete[filePath] = DateTime.UtcNow.Add(delay);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(300), stoppingToken); 
            
            foreach (var file in _filesToDelete)
            {
                if (DateTime.UtcNow >= file.Value)
                {
                    try
                    {
                        if (File.Exists(file.Key))
                        {
                            File.Delete(file.Key);
                            _filesToDelete.TryRemove(file.Key, out _);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {file.Key}: {ex.Message}");
                    }
                }
            }
        }
    }
}
