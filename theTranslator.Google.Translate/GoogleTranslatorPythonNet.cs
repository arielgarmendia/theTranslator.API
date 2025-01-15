using Python.Runtime;
using System.Reflection;

namespace theTranslator.Google.Translate
{
    public static class GoogleTranslatorPythonNet
    {
        private static string PyPath { get; } = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"");
        public static async Task<string> TranslateAsync(string text, string langTgt = "auto", string langSrc = "auto")
        {
            try
			{
                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();

                    dynamic sys = Py.Import("sys");
                    sys.path.append(PyPath);
                }

                dynamic GoogleTranslatorModule = Py.Import("GoogleTranslator");
                dynamic PyGoogleTranslator = GoogleTranslatorModule.google_translator();

                return PyGoogleTranslator.translate(text, langTgt, langSrc);
            }
			catch (Exception)
			{
                return "Error: Failed to translate";
			}
        }
    }
}

