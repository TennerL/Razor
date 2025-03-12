using Microsoft.EntityFrameworkCore;
using Razor.Components.Models;

namespace Razor.Components.Services
{
    public class FileAccessService
    {
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
