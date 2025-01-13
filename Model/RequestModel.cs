namespace theTranslator.API.Model
{
    public class RequestModel
    {
        public int SourceLanguage { get; set; }
        public int DestinationLanguage{ get; set; }
        public string? TextToTranslate { get; set; }

        public RequestModel() { }
    }
}
