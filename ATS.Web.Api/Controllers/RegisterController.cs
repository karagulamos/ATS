using ATS.Web.Api.Security;
using Library.Core.Bootstrapper;
using Library.Persistence;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http;

namespace ATS.Web.Api.Controllers
{
    [RoutePrefix("before/enter")]
    public class RegisterController : ApiController
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private IUserService _userAuthenticationService;

        public RegisterController()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
       
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(AppUser user, string password)
        {
            try
            {
                user.UserName = user.Email;

                if (await _userAuthenticationService.UserExistsAsync(user.UserName))
                    return BadRequest("User already exists");

                if(await _userAuthenticationService.CreateUserAsync(user, password))
                {
                    return Ok("User successfully created.");
                }

                return BadRequest("User creation failed.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error creating user " + user.UserName + " - " + ex.Message);
            }
        }
    }
}
