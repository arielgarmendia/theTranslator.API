using Microsoft.Scripting.Hosting;
using System.Reflection;

namespace theTranslator.Google.Translate
{
    public class GoogleTranslatorIronPython
    {
        public async Task<string> TranslateAsync(string text, string langTgt = "auto", string langSrc = "auto")
        {
			try
			{
                string pyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"PythonCode\GoogleTranslator.py");
                string libPath1 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"lib");

                ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
                var paths = engine.GetSearchPaths();

                paths.Add(libPath1);

                engine.SetSearchPaths(paths);

                ScriptSource source = engine.CreateScriptSourceFromFile(pyPath);
                ScriptScope scope = engine.CreateScope();

                source.Execute(scope);

                dynamic pyGoogleTranslator = scope.GetVariable("google_translator");
                dynamic googleTranslator = pyGoogleTranslator();

                return await googleTranslator.translate(text, langTgt, langSrc);
            }
			catch (Exception)
			{
                return "Error: Failed to translate";
			}

            return string.Empty;
        }
    }
}

