namespace theTranslator.Service.Model
{
    public class RequestModel
    {
        public string SourceLanguage { get; set; }
        public string DestinationLanguage{ get; set; }
        public string TextToTranslate { get; set; } = string.Empty;

        public RequestModel() { }
    }
}
