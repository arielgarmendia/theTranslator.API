using theTranslator.Google.Translate;

namespace theTranslator.Service
{
    public static class Translate
    {
        public async static Task<string> ExecuteAsync(string TextToTranslate, string DestinationLanguage, string SourceLanguage, int TranslationService = 1)
        {
            switch (TranslationService)
            {
                case 1:
                    return await GoogleTranslatorPythonNet.TranslateAsync(TextToTranslate, DestinationLanguage, SourceLanguage);
                default:
                    return "Error: Translation Service needs to be provided.";
            }            
        }
    }
}
