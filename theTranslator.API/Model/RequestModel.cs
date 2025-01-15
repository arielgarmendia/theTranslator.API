namespace theTranslator.API.Model
{
    /// <summary>
    /// Translation Integration provider.
    /// </summary>
    public enum TranslationServices 
    {
        /// <summary>
        /// Google Translate service.
        /// </summary>
        GoogleTranslate = 1 
    }

    /// <summary>
    /// Request model for translation process.
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// Translation Language Source.
        /// </summary>
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Traslation Language Target.
        /// </summary>
        public string DestinationLanguage{ get; set; }

        /// <summary>
        /// Text to Translate.
        /// </summary>        
        public string TextToTranslate { get; set; } = string.Empty;

        /// <summary>
        /// Translation Integration provider.
        /// </summary>
        public TranslationServices TranslationService { get; set; } = TranslationServices.GoogleTranslate;

        public RequestModel() { }
    }
}
