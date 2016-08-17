using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http;
using ATS.Web.Api.Security;
using Library.Core.Bootstrapper;
using Library.Persistence;

namespace ATS.Web.Api.Controllers
{
    [RoutePrefix("before/enter")]
    public class RegisterController : ApiController
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private IUserAuthenticationService _userAuthenticationService;

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

                if (await _userAuthenticationService.UserExists(user.UserName))
                    return BadRequest("User already exists");

                if(await _userAuthenticationService.CreateUser(user, password))
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
