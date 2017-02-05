using ATS.Web.Api.Security;
using Library.Core.Bootstrapper;
using Library.Core.Persistence.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http;

namespace ATS.Web.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/identity")]
    public class IdentityController : ApiController
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        IUserService _userAuthenticationService;

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private ICandidateRepository _candidateRepository;

        public IdentityController()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        [Route("details")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(new
                {
                    CurrentUser = await _userAuthenticationService.GetCurrentUserDetailsAsync(User.Identity.GetUserId()),
                    TotalCandidates = _candidateRepository.GetDataCount()
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error getting admin details - " + ex.Message);
            }
        }

    }
}