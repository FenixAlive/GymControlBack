using gymControl.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace gymControl.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetUsers()
        {
            try
            {
                return NoContent();
            }
            catch (Exception)
            {

                throw new System.Web.Http.HttpResponseException(HttpStatusCode.NotFound);
            }
            
        }
    }
}
