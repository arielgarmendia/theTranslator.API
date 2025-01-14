using Python.Included;
using Python.Runtime;
using System.Reflection.Metadata.Ecma335;

namespace theTranslator.API.Included
{
    public static class PythonLibraries
    {
        public static async Task<bool> Add()
        {
            try
            {
                // install in local directory
                Installer.InstallPath = Path.GetFullPath(".");

                // install the embedded python distribution
                await Installer.SetupPython();

                // install pip3 for package installation
                await Installer.TryInstallPip();

                await Installer.PipInstallModule("json");
                await Installer.PipInstallModule("requests");
                await Installer.PipInstallModule("random");
                await Installer.PipInstallModule("re");
                await Installer.PipInstallModule("urllib");
                await Installer.PipInstallModule("urllib3");
                await Installer.PipInstallModule("logging");

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
