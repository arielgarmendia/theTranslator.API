namespace theTranslator.Service.Model
{
    public enum TranslationServices { GoogleTranslate = 1 }
    public class RequestModel
    {
        public string SourceLanguage { get; set; }
        public string DestinationLanguage{ get; set; }
        public string TextToTranslate { get; set; } = string.Empty;
        public TranslationServices TranslationService { get; set; } = TranslationServices.GoogleTranslate;

        public RequestModel() { }
    }
}
