using Microsoft.EntityFrameworkCore;
using Razor.Components.Models;

namespace Razor.Components.Services
{
    public class DataSourceService
    {
        
        public async Task<List<DataSources>> GetDataSources()
        {
            return await _context.DataSources.ToListAsync();
        }
        private readonly ApplicationDbContext _context;
        public DataSourceService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddDataSource(string sourcePath)
        {
            var dataSource = new DataSources { SourcePath = sourcePath };
            _context.DataSources.Add(dataSource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataSource(DataSources dataSource)
        {
            var entry = await _context.DataSources
                .FirstOrDefaultAsync(r => r.SourcePath == dataSource.SourcePath);
            if(entry != null)
            {
                _context.DataSources.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
