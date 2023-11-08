using BNPISINClient.Interfaces;
using BNPISINClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace BNPISINClient.Controllers
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

        [HttpGet]
        public IActionResult Index(List<string> codeList)
        {
            var response = _service.ExecuteAsync(codeList);
            return Json(response);
        }
    }
}
