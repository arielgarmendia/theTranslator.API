using Microsoft.AspNetCore.Mvc;
using System.Net;
using theTranslator.API.Model;

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
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public String Post([FromBody] RequestModel request)
        {
            return "Nothing to translate yet.";
        }
    }
}
