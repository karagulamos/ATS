using Library.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ATS.Web.Api.Security
{
    [Export(typeof(IUserService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AtsUserService : IUserService
    {
        private const string RoleName = "admin";

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleStore<IdentityRole> _roleStore;
        public AtsUserService()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AtsDbContext())) { };
            _roleStore = new RoleStore<IdentityRole>(new AtsDbContext());
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _userManager.FindAsync(username, password);
            return user != null;
        }

        public async Task<IList<string>> GetUserRolesAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.GetRolesAsync(user.Id);
        }

        public async Task<string> GetUserIdAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return user.Id;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return user != null;
        }

        private Task<bool> RoleExistsAsync(string role)
        {
            return _roleStore.Roles.AnyAsync(r => r.Name.ToLower() == role);
        }

        public async Task<bool> CreateUserAsync(AppUser user, string password)
        {
            if (!await RoleExistsAsync(RoleName))
            {
                await _roleStore.CreateAsync(new IdentityRole
                {
                    Name = RoleName
                });
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user.Id, RoleName);
            }

            return result.Succeeded;
        }

        public async Task<LoggedOnUser> GetCurrentUserDetailsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return new LoggedOnUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}