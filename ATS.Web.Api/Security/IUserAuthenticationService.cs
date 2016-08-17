using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Persistence;

namespace ATS.Web.Api.Security
{
    public interface IUserAuthenticationService
    {
        bool ValidateUserCredentials(string username, string password);
        IList<string> GetUserRoles(string username);
        string GetUserId(string username);

        Task<bool> UserExists(string username);
        Task<bool> CreateUser(AppUser user, string password);

        LoggedOnUser GetCurrentUserDetails(string userId);
    }

    public class LoggedOnUser
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}