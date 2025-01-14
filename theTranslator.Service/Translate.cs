using theTranslator.Google.Translate;
using theTranslator.Service.Model;

namespace theTranslator.Service
{
    public static class Translate
    {
        public async static Task<string> ExecuteAsync(RequestModel request)
        {
            return await GoogleTranslatorPythonNet.TranslateAsync(request.TextToTranslate, request.DestinationLanguage, request.SourceLanguage);
        }
    }
}
