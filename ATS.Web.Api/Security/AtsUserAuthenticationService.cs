using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Library.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ATS.Web.Api.Security
{
    [Export(typeof(IUserAuthenticationService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AtsUserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleStore<IdentityRole> _roleStore;
        public AtsUserAuthenticationService()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AtsDbContext())){};
            _roleStore = new RoleStore<IdentityRole>(new AtsDbContext());
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            var user = _userManager.Find(username, password);
            return user != null;
        }

        public  IList<string> GetUserRoles(string username)
        {
            var user = _userManager.FindByEmail(username);
            return _userManager.GetRoles(user.Id);
        }

        public string GetUserId(string username)
        {
            var user =  _userManager.FindByEmail(username);
            return user.Id;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return user != null;
        }

        bool RoleExists(string role)
        {
            var roleExist = _roleStore.Roles.Any(r => r.Name.ToLower() == "admin");
            return roleExist;
        }

        public async Task<bool> CreateUser(AppUser user, string password)
        {
            var roleName = "admin";

            if (!RoleExists(roleName))
            {
                await _roleStore.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user.Id, roleName);
                return result.Succeeded;
            }

            return false;
        }

        public LoggedOnUser GetCurrentUserDetails(string userId)
        {
            var user = _userManager.FindById(userId);

            return new LoggedOnUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}