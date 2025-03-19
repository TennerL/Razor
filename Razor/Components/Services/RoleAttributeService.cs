using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Razor.Components.Models;

namespace Razor.Components.Services
{
    public class RoleAttributeService
    {
        private readonly ApplicationDbContext _context;

        public RoleAttributeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleAttributes>> GetRoleAttributes()
        {
            return await _context.RoleAttributes.ToListAsync();
        }

        public async Task<RoleAttributes?> GetRoleAttribute(string roleId, string rule)
        {
            return await _context.RoleAttributes
                .FirstOrDefaultAsync(a => a.RoleId == roleId && a.Rule == rule);
        }

        public async Task AddRoleAttributes(string roleId, string rule, bool value)
        {
            var existingAttribute = await GetRoleAttribute(roleId, rule);

            if (existingAttribute != null)
            {
                existingAttribute.Value = value;
            }
            else
            {
                var newAttribute = new RoleAttributes { RoleId = roleId, Rule = rule, Value = value };
                _context.RoleAttributes.Add(newAttribute);
            }

            await _context.SaveChangesAsync();
        }
    }
}
