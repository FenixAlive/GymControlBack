using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace gymControl.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> GetUsers( [FromQuery] UserQuery query = null)
        {
            try
            {
                var claims = HttpContext.User.Identity as ClaimsIdentity;
                string id = null;
                if(claims != null && claims.Claims.Count() > 0)
                {
                    id = claims.Claims.FirstOrDefault(v => v.Type == "id").Value;
                }
                else
                {
                    return BadRequest("There are errors on the Authentication Token");
                }
                var users = await _service.GetUsers(id, query);
                string json = JsonConvert.SerializeObject(users);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
