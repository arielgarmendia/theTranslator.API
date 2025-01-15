using Microsoft.AspNetCore.Mvc;
using theTranslator.API.Model;
using theTranslator.Service;

namespace theTranslator.API.Controllers
{
    /// <summary>
    /// Main controller to access translation endpoints.
    /// </summary>
    [ApiController]
    [Route("api/translator")]
    public class TranslatorController : ControllerBase
    {
        private readonly ILogger<TranslatorController> _logger;

        public TranslatorController(ILogger<TranslatorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Translates a text using a source language and a target language.
        /// </summary>
        /// <returns>The translated text in target language.</returns>
        [Route("/Translate")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RequestModel request)
        {
            return new JsonResult(new
            {
                Message = await Translate.ExecuteAsync(request.TextToTranslate, request.DestinationLanguage, request.SourceLanguage, (int)request.TranslationService)
            });
        }
    }
}
