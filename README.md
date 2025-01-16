# theTranslator.API
Repo for dockerised ***theTranslator.API*** project

### Requirements

- Local install of *Docker Desktop* app. Up and running.
- A local copy of a *Visual Studio* suite, configured to process *.Net 8* apps.

### Description

- *.Net 8 RESTful WebAPI*.
- Docummented using *Swagger* and *OpenAPI*, default landing page when executed.
- *WebAPI* app has a fixed https port value assigned, to allow *CORS* operations between 2 domain separated apps: Web app and this *API* service.
- *Docker* container configured for *Windows OS*, to make it simpler.
- Single async *GET* entry endpoint to perform the translation process.
```csharp
    [Route("/Translate")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RequestModel request)
```
- ***theTranslator.Service*** project/layer acts as a translation integration service selector, allowing multiple translator integrations:
    - Default now is *Google Translate*.
    - This project could be designed as a *Nuget* package, maintaining a single point for future translation integration services.

### Execution Plan

- Clone the repository in your local machine.
- Open solution with *Visual Studio*.
- Make a rebuild to load any needed package.
- Just press *F5* to run the *WebAPI* app in a *Docker Container*. 
- ***Swagger*** doc will appear in a new browser window, meaning the *API* is ready.
- *WebAPI* app will work independently of the Web app.

### Development path and issues found during the development process (this is the funny part!... or maybe not)

- ***theTranslator.Google.Translate*** project/layer:
    - Here is where everything happens.
    - I´ve kept aligned to the idea of using the suggested *Python library*.
    - Using the *Google Translate API* requires an access token, so this will be avoided.
    - First approach:
        - Convert the *Python* source code to *C#* code.
        - This was succesfull but when trying to use the generated code, I had connection issues, so I assumed that conversion wasm´t good enough.
    - Second approach:
        - Combine *C#* code and *Python* code in the same *.Net* project, so we have also multilanguage processing, *.Net* framework accessing and executing *Python* code: 
            - First approach:
                - Use of *IronPython Nuget* package.
                - I´ve managed to call the Python code, but started having a *module not found* error.
                - The lack of *Python´s "requests"* module was the main cause of failure.
                - *IronPython* only processes few standard *Python* modules and *"requests"* is not one of the provided in the implementation.
                - *theTranslator.Google.Translator/GoogleTranslatorIronPython.cs* is the example of this failed attempt.
                - I´ve kept it in the project as an example of a possible implementation.
            - Second approach:
                - Use of *Python.Included Nuget* package.
                - This package allows us to use another implementation called *Python.Net* which uses *Python´s* full framework.
                - Using *Python.Included Nuget* allows us to download modules on demand, see *theTranslator.API/Included/PythonLibraries.cs**, called at *WebAPI* startup/program entry point.
                
                ```csharp
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
                ```
                - This allows us to one-time download the modules used in *theTranslator.Google.Translator/GoogleTranslator.py* implementation.
                - See *theTranslator.Google.Translator/GoogleTranslatorPythonNet.cs* for the translation process using *Python.Included* and *Python.Net*
                - This approach works, but not as well as we desire.
                - **Problem**: I´ve managed to only make two translations using the *Web** app connecting to the *WebAPI*. At third attempt the *Python* code, called from *.Net* stops working, no response. I suspect there´s a problem when accessing *Google Tranlate* from an unofficial implementation.

                ```csharp
                dynamic GoogleTranslatorModule = Py.Import("GoogleTranslator");
                dynamic PyGoogleTranslator = GoogleTranslatorModule.google_translator();

                return PyGoogleTranslator.translate(text, langTgt, langSrc);
                ```
        - Third approach:
            - Maybe there´s a third option here, who knows...
            
              (⌐■_■)
