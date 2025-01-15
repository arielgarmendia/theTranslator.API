# theTranslator.API
Repo for dockerised theTranslator.API project

### Requirements

- Local install of Docker Desktop app. Up and running.
- A local copy of a Visual Studio suite, configured to process .Net 8 apps.

### Description

- .Net 8 RESTful Web API.
- Docummented using Swagger, default landing page when executed.
- Web API app has a fixed https port value assigned, to allow CORS operations between 2 domain separated apps: Web app and this API service.
- Docker container configured for Windows OS, to make it simpler.
- Single async GET entry endpoint to perform the translation process.
```csharp
    [Route("/Translate")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RequestModel request)
```

### Execution

- Clone the repository in your local machine.
- Open solution with Visual Studio.
- Make a rebuild to load any needed package.
- Just press F5 to run the Web API app in a Docker Container. 
- Swagger doc will appear in a new browser window, meaning the API is ready.
- Web API app will work independently of the Web app.

### Important Notes