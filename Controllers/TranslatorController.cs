using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using theTranslator.Service;
using theTranslator.Service.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace theTranslator.API.Controllers
{
    [ApiController]
    [Route("api/translator")]
    public class TranslatorController : ControllerBase
    {
        private readonly ILogger<TranslatorController> _logger;

        public TranslatorController(ILogger<TranslatorController> logger)
        {
            _logger = logger;
        }

        [Route("/Translate")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RequestModel request)
        {
            return new JsonResult(new
            {
                Message = await Translate.ExecuteAsync(request)
            });
        }
    }
}
