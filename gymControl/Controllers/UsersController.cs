using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpGet]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> GetUser(int? userId = null)
        {
            try
            {
                var claims = HttpContext.User.Identity as ClaimsIdentity;
                string id = null;
                if (claims != null && claims.Claims.Count() > 0 && userId != null)
                {
                    id = claims.Claims.FirstOrDefault(v => v.Type == "id").Value;
                }
                else
                {
                    return BadRequest("There are errors on the Authentication Token");
                }
                var user = await _service.GetUser(id, (int)userId);
                string json = JsonConvert.SerializeObject(user);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IActionResult> PostUser([FromBody] User user = null)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest("User invalid from payload");
                }
                var userResponse = await _service.AddUser(user);
                string json = JsonConvert.SerializeObject(userResponse);
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
        public async Task<IActionResult> PutUser([FromBody] User user = null)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User invalid from payload");
                }
                var userResponse = await _service.EditUser(user);
                string json = JsonConvert.SerializeObject(userResponse);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUser( int? userId = null)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest("invalid userId from url");
                }
                var userResponse = await _service.RemoveUser((int)userId);
                string json = JsonConvert.SerializeObject(userResponse);
                return Content(json, "application/json");
            }
            catch (Exception _)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
