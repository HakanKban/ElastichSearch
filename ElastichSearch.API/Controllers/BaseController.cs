using ElastichSearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.Status == System.Net.HttpStatusCode.NotExtended)
            {
                return new ObjectResult(null) { StatusCode = response.Status.GetHashCode() };
            }
            return new ObjectResult(response) { StatusCode = response.Status.GetHashCode() };
        }
    }
}
