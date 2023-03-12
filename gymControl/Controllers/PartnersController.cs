using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace gymControl.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]/")]
    public class PartnersController : Controller
    {
        private readonly IPartnerService _partnerService;
        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPartner([FromQuery] PartnerQuery query = null)
        {
            try
            {
                var partner = await _partnerService.GetPartners(query);
                string json = JsonConvert.SerializeObject(partner);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
            
        }

        [HttpGet]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticatePartner([FromQuery] string? username = null, [FromQuery] string? passwd = null)
        {
            try
            {
                if(username == null || passwd == null)
                {
                    return BadRequest("username and passwd are required");
                }
                var partner = await _partnerService.AuthenticatePartner(username, passwd);
                if(partner == null)
                {
                    return NotFound();
                }
                string json = JsonConvert.SerializeObject(partner);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> PostPartner([FromBody] Partner partner = null)
        {
            try
            {
                if(partner == null)
                {
                    return BadRequest("Payload null or invalid, It should be a valid partner object");
                }
                var result = await _partnerService.AddPartner(partner);
                string json = JsonConvert.SerializeObject(result);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> RemovePartner([FromBody] Partner partner)
        {
            try
            {
                var result = await _partnerService.EditPartner(partner);
                string json = JsonConvert.SerializeObject(result);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemovePartner(int? id = null)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("You need to pass an Id to URL");
                }
                await _partnerService.RemovePartner(id);
                return NoContent();
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
