using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginPartner([FromBody] LoginUser loginUser = null)
        {
            try
            {
                if (loginUser == null || loginUser?.UserName?.Length == 0 || loginUser?.Password?.Length < 4)
                {
                    return BadRequest("Payload null or invalid, It should be a valid login object");
                }

                var result = await _partnerService.LoginPartner(loginUser!);
                if(result == null)
                {
                    return NotFound("Invalid credentials");
                }
                string json = JsonConvert.SerializeObject(result);
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
        [Authorize]
        [Route("")]
        public async Task<IActionResult> UpdatePartner([FromBody] Partner partner)
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
        [Authorize]
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
