using System;
using System.ComponentModel.Composition;
using System.Web.Http;
using ATS.Web.Api.Security;
using Library.Core.Bootstrapper;
using Library.Core.Persistence.Repositories;
using Microsoft.AspNet.Identity;

namespace ATS.Web.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/identity")]
    public class IdentityController : ApiController
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        IUserAuthenticationService _userAuthenticationService;

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private ICandidateRepository _candidateRepository;

        public IdentityController()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        [Route("details")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(new
                 {
                     CurrentUser = _userAuthenticationService.GetCurrentUserDetails(User.Identity.GetUserId()),
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