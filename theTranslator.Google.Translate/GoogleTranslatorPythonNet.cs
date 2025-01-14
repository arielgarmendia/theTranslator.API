using Python.Runtime;
using System.Reflection;

namespace theTranslator.Google.Translate
{
    public static class GoogleTranslatorPythonNet
    {
        public static async Task<string> TranslateAsync(string text, string langTgt = "auto", string langSrc = "auto")
        {
            var result = string.Empty;
            string pyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"");

            try
			{
                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();

                    dynamic sys = Py.Import("sys");
                    sys.path.append(pyPath);
                }

                dynamic GoogleTranslatorModule = Py.Import("GoogleTranslator");
                dynamic PyGoogleTranslator = GoogleTranslatorModule.google_translator();

                result = PyGoogleTranslator.translate(text, langTgt, langSrc);

                return result;
            }
			catch (Exception)
			{
                return "Error: Failed to translate";
			}
        }
    }
}

