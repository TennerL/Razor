using Microsoft.EntityFrameworkCore;
using Razor.Components.Models;

namespace Razor.Components.Services
{
    public class FileAccessService
    {
        public async Task<List<FileAccessRule>> GetAccessRulesFromDb()
        {
            return await _context.FileAccessRules.ToListAsync();
        }

        public async Task DeleteAccessRule(FileAccessRule rule)
        {
            var entry = await _context.FileAccessRules
                .FirstOrDefaultAsync(r => r.FilePath == rule.FilePath && r.UserId == rule.UserId && r.Permission == rule.Permission);
            if (entry != null)
            {
                _context.FileAccessRules.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        private readonly ApplicationDbContext _context;

        public FileAccessService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAccessRule(string filePath, string userId, string permission)
        {
            var rule = new FileAccessRule { FilePath = filePath, UserId = userId, Permission = permission };
            _context.FileAccessRules.Add(rule);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> HasAccess(string filePath, string userId, string permission)
        {
            return await _context.FileAccessRules
                .AnyAsync(r => r.FilePath == filePath && r.UserId == userId && r.Permission == permission);
        }
    }

}
