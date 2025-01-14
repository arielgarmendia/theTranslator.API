using theTranslator.Google.Translate;
using theTranslator.Service.Model;

namespace theTranslator.Service
{
    public static class Translate
    {
        public async static Task<string> ExecuteAsync(RequestModel request)
        {
            var translator = new GoogleTranslator();

            return await translator.TranslateAsync(request.TextToTranslate, request.DestinationLanguage, request.SourceLanguage);
        }
    }
}
