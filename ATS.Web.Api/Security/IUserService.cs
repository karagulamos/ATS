using Library.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATS.Web.Api.Security
{
    public interface IUserService
    {
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task<IList<string>> GetUserRolesAsync(string username);
        Task<string> GetUserIdAsync(string username);

        Task<bool> UserExistsAsync(string username);
        Task<bool> CreateUserAsync(AppUser user, string password);

        Task<LoggedOnUser> GetCurrentUserDetailsAsync(string userId);
    }
}