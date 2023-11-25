using ApiClient.Interfaces;
using ApiClient.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace ApiClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        private readonly ISecurityService _service;

        public SecurityController(ISecurityService service)
        {
            _service = service;
        }


        /// <summary>
        /// Security request
        /// </summary>
        /// <param name="securities">List of ISIN</param>
        /// <returns>List of codes and result, to check whether it was processed</returns>
        /// <remarks>
        [HttpPost]
        [SwaggerISINExamples]
        [Produces("application/json")]
        public IActionResult Index(List<string> securities)
        {
            var response = _service.ExecuteAsync(securities);
            return Json(response);
        }
    }



    

    
}
   

