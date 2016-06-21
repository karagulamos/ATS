using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using Library.Core.Bootstrapper;
using Library.Data;
using Library.Models;
using Library.Services.Helper;

namespace ATS.Web.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/candidates")]
    public class CandidateController : ApiController
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private ICandidateRepository _candidateRepository; 

        public CandidateController()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);  // import
        }

        [Route("query")]
        [HttpGet]
        public IHttpActionResult Get(ODataQueryOptions<Candidate> queryOptions)
        {
            try
            {
                var results = (IQueryable<Candidate>)queryOptions.ApplyTo(_candidateRepository.GetQueryableCandidates());
                return Ok(new PageResult<Candidate>(results, queryOptions.Request.ODataProperties().NextLink, queryOptions.Request.ODataProperties().TotalCount));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving candidate list - " + ex.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(_candidateRepository.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving candidate " + id + " - " + ex.Message);
            }
        }

        [Route("edit")]
        [HttpPost]
        public IHttpActionResult Edit(Candidate candidate)
        {
            try
            {
                _candidateRepository.DetachAndUpdate(candidate.CandidateId, candidate);

                return Ok("Candidate profile successfully updated.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error updating candidate profile - " + ex.Message);
            }
        }

        [Route("delete/{candidateId}")]
        [HttpGet]
        public IHttpActionResult Delete(int candidateId)
        {
            try
            {
                var candidate = _candidateRepository.Get(candidateId);

                if (candidate != null)
                {
                    _candidateRepository.Delete(candidate);
                    return Ok("Candidate successful deleted.");
                }

                return BadRequest("Error: Candidate has already been deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving candidate - " + ex.Message);
            }
        }

        [Route("preview/{candidateId}")]
        [HttpGet]
        public HttpResponseMessage Preview(int candidateId)
        {
            try
            {
                if (candidateId <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                var attachment = _candidateRepository.GetCandidatePath(candidateId);

                if (attachment == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var fileStream = new FileStream(attachment.FilePath, FileMode.Open, FileAccess.Read);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(fileStream)
                };

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = attachment.OriginalFileName
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.Add("X-ATS-File", attachment.OriginalFileName);

                return result;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Route("states")]
        [HttpGet]
        public IHttpActionResult States()
        {
            return Ok(ResumeFilterHelper.GetStates());
        }
    }
}