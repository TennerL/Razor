using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razor.Components.Models;
using Razor.Components.Pages.Account;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor.Components.Services
{
    public class UserManagementService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string role = roles.FirstOrDefault() ?? "Keine Rolle";
                userList.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = role
                });
            }
            return userList;
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roleList = await _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return roleList.Any() ? roleList : new List<RoleDto> { new RoleDto { Name = "Keine Rollen vorhanden" } };
        }


        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return false;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains(model.NewRole))
            {
                return true;
            }

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return false;
            }

            var addResult = await _userManager.AddToRoleAsync(user, model.NewRole);
            return addResult.Succeeded;
        }
        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, roleName);
        }


    }

    public class UpdateRoleModel
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}
