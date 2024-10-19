[![dotnet core - build](https://github.com/ardalis/ApiEndpoints/workflows/dotnet%20core%20-%20build/badge.svg)](https://github.com/ardalis/ApiEndpoints/actions?query=workflow%3A%22dotnet+core+-+build%22) 
[![Nuget](https://img.shields.io/nuget/v/Ardalis.ApiEndpoints)](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)
[![Nuget](https://img.shields.io/nuget/dt/Ardalis.ApiEndpoints)](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)

<a href="https://twitter.com/intent/follow?screen_name=ardalis">
    <img src="https://img.shields.io/twitter/follow/ardalis.svg?label=Follow%20@ardalis" alt="Follow @ardalis" />
</a> &nbsp; <a href="https://twitter.com/intent/follow?screen_name=nimblepros">
    <img src="https://img.shields.io/twitter/follow/nimblepros.svg?label=Follow%20@nimblepros" alt="Follow @nimblepros" />
</a>

# ASP.NET Core API Endpoints

A project for supporting API Endpoints in ASP.NET Core web applications.

## Fast Endpoints

If you're on .NET 6 or later (if you're no longer on .NET 4.x you should be on at least .NET 8) you're probably better off using Minimal APIs than Controllers. MVC is a [dull edge technology](https://ardalis.com/technology-edges-bleeding-leading-rusting/) at this point. If you like the organization used by this library and the [REPR Pattern](https://deviq.com/design-patterns/repr-design-pattern) in general, I am now recommending the [FastEndpoints](https://fast-endpoints.com/) library. It has a *lot* of additional functionality you can use or not, but its core api endpoint functionality is very solid and fast.

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Discover a Practical Example

If you're eager to dive into a practical example of using `Ardalis.ApiEndpoints`, check out our [Getting Started guide](#3-getting-started). This guide walks you through setting up your environment and creating your first API endpoint using the latest features in version 4.x.

## Upgrade to 4.x Notes

The fluent generics and base types involved in ApiEndpoints were updated in version 4.x, resulting in breaking changes. The updates required should be pretty straightforward, and have a few additional features that weren't supported in previous versions.

The two main changes introduced in v4 are:

- Base classes should now use `EndpointBaseSync` or `EndpointBaseAsync`
- `WithResponse` has been modified to `WithResult` or `WithActionResult`

The *result* of an endpoint corresponds to the return type from the `Handle` method. Since ASP.NET Core MVC refers to these as some variation of Action*Result*, that's the term we are using in this package now as well. The *Response* your endpoint may return refers to any data/DTO that is being sent to the client as part of the *Result*. If you wish to preserve your existing v3 functionality that specified `WithResponse<T>` you should be able to replace all such occurrences with `WithActionResult<T>`. However, if you need to specify a different kind of Result, such as a `FileResult`, you can now use something like `WithResult<FileResult>` to achieve this.

An endpoint that previously inherited from the synchronous `BaseEndpoint` should now inherit from `EndpointBaseSync`. Additionally, the `WithResponse` option now has optional non-generic versions, but if you were intending to return an `ActionResult<T>` you would now use `WithActionResult<T>` in your class definition, like so:

```diff
- public class ForecastEndpoint : BaseEndpoint
-     .WithRequest<ForecastRequestDto>
-     .WithResponse<IEnumerable<WeatherForecast>>
+ public class ForecastEndpoint : EndpointBaseSync
+     .WithRequest<ForecastRequestDto>
+     .WithActionResult<IEnumerable<WeatherForecast>>
```

The above change typically would not require any change to the `Handle` method. Endpoints that inherited from `BaseAsyncEndpoint` would now use `EndpointBaseAsync`. You can also just inherit from `EndpointBase` directly (without the `.With*` additions) which will provide you with a controller with a single Handle method without restrictions on parameter amount and type, if you need more flexibility than the fluent generic interface provides.

## Upgrade to 3.x Notes

For version 3.0 we implemented a new way to define the base classes using "fluent generics". You can [watch a video of what you need to know to apply them to your site here](https://www.youtube.com/watch?v=hKiuj0huEI4&ab_channel=Ardalis).

## Table of Contents

[1. Motivation](#1-motivation)

[2. Introducing ASP.NET Core API Endpoints](#2-introducing-aspnet-core-api-endpoints)

[3. Getting Started](#3-getting-started)

- [Set Up an Empty Web API Project](#set-up-an-empty-web-api-project)

- [Adding Controllers with Ardalis.ApiEndpoints](#adding-controllers-with-ardalisapiendpoints)

- [Adding common endpoint groupings using Swagger](#adding-common-endpoint-groupings-using-swagger)

- [File Upload Example](#file-upload-example)

[4. Animated Screenshots](#4-animated-screenshots)

[5. Open Questions](#5-open-questions)

[6. Roadmap](#6-roadmap)

[7. Related Articles](#7-related-articles)

[8. Videos and Podcasts](#8-videos-and-podcasts)

[9. Related / Similar Projects](#9-related--similar-projects)

[10. Projects Using ApiEndpoints](#10-projects-using-apiendpoints)

[11. Success Stories and Testimonials](#11-success-stories-and-testimonials)

## 1. Motivation

MVC Controllers are essentially an antipattern. They're dinosaurs. They are collections of methods that never call one another and rarely operate on the same state. They're not cohesive. They tend to become bloated and to grow out of control. Their private methods, if any, are usually only called by a single public method. Most developers recognize that controllers should be as small as possible ([unscientific poll](https://twitter.com/ardalis/status/1223312390391058432)), but they're the only solution offered out of the box, so that's the tool 99% of ASP.NET Core developers use.

You can use tools like MediatR to mitigate the problem. You can read a [detailed article about how to migrate from Controllers to Endpoints using MediatR](https://ardalis.com/moving-from-controllers-and-actions-to-endpoints-with-mediatr). The short version is that MediatR enables you to have single-line action methods that route commands to handlers. This is objectively a better approach, resulting in more cohesive classes that better follow OO principles. But what if you didn't even need that extra plumbing?

That's what ASP.NET Core API Endpoints are all about.

### Side note: Razor Pages

The .NET team already did *this exact thing* with razor pages. They recognized that dealing with Views, ViewModels, Controllers, and Actions was way more complicated than necessary. It required a developer to jump around between at least 3 (and often more) different folders in order to add or modify a new page/view to their project. Razor pages addressed this by rethinking the model for page-based ASP.NET Core MVC endpoints.

Razor Pages group each page's razor markup, its related action(s), and its model into two linked files. It uses the same MVC features as the rest of the platform, so you still get routing, model binding, model validation, filters, the works. You literally give up nothing. But now when you need to add or modify a page you need to look at exactly 2 files, which are linked in the IDE so you don't need to scroll around the file system looking for them.

## 2. Introducing ASP.NET Core API Endpoints

**ASP.NET Core API Endpoints are essentially Razor Pages for APIs.** They break apart bloated controllers and group the API models used by individual endpoints with the endpoint logic itself. They provide a simple way to have a single file for the logic and linked files for the model types.

When working with ASP.NET Core API Endpoints your project won't need any Controller classes. You can organize the Endpoints however you want. By feature. In a giant Endpoints folder. It doesn't matter - they'll work regardless of where you put them.

Most REST APIs have groups of endpoints for a given resource. In Controller-based projects you would have a controller per resource. When using API Endpoints you can simply create a folder per resource, just as you would use folders to group related pages in Razor Pages.

**Instead of Model-View-Controller (MVC) the pattern becomes Request-EndPoint-Response(REPR). The REPR (reaper) pattern is much simpler and groups everything that has to do with a particular API endpoint together.** It follows SOLID principles, in particular SRP and OCP. It also has all the benefits of feature folders and better follows the Common Closure Principle by grouping together things that change together.

## 3. Getting Started

### Set Up an Empty Web API Project

#### Overview

When starting a new Web API project using .NET, you might want to begin with an empty project structure to have more control over dependencies and configurations. 

#### Setting Up the Project

1. **Create the Project:**

   Start by creating a new empty web API project. You can do this using the .NET CLI:

   ```bash
   dotnet new web -n MyWebApi
   cd MyWebApi
   ```

   This creates a basic project structure for a web application.

2. **Update Program.cs:**

   Open the `Program.cs` file, which serves as the entry point for your application. By default, it might contain the following code:

   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   var app = builder.Build();

   // app.MapGet("/", () => "Hello World!");

   app.Run();
   ```

   Remove the `app.MapGet("/", () => "Hello World!");` line as it sets up a minimal endpoint which we'll replace with controller routing.

3. **Configure Services and Routing:**

   Modify the `Program.cs` file to include controller services and routing:

   ```csharp
   var builder = WebApplication.CreateBuilder(args);

   // Add controllers
   builder.Services.AddControllers();

   var app = builder.Build();

   // Map controllers to endpoints
   app.MapControllers();

   app.Run();
   ```

   - **`builder.Services.AddControllers()`**: Adds services required for controllers to handle HTTP requests.
   - **`app.MapControllers()`**: Maps controllers to appropriate endpoints based on routing attributes and conventions.
   
   These two methods are required for the endpoint to work.

### Adding Controllers with Ardalis.ApiEndpoints

1. **Install Ardalis.ApiEndpoints NuGet Package**

   Add the `Ardalis.ApiEndpoints` package to your ASP.NET Core web project. You can do this using the NuGet Package Manager or via the .NET CLI:

   ```bash
   dotnet add package Ardalis.ApiEndpoints
   ```

2. **Create Endpoint Classes**

   Create your endpoint classes by inheriting from `EndpointBaseSync` or `EndpointBaseAsync`, and add `.WithRequest<TRequest>` or `.WithResult<TResponse>`, or both, depending on whether your endpoint accepts input (POST) or simply returns a response (GET).

3. **Implement Handle Method**

   Implement the `Handle` method from the base class (`EndpointBaseSync`) in your endpoint class. This method contains the logic to process the request and return the response.

   ```csharp
   public class MyEndpoint : EndpointBaseSync
    .WithRequest<string>
    .WithActionResult<IActionResult>
    {
        [HttpGet("my-endpoint")]
        public override ActionResult<IActionResult> Handle(string request)
        {
            // Your logic here

            return Ok();
        }
    }
   ```

4. **Add Routing Attributes**

   Decorate your `Handle` method with `[HttpGet]`, `[HttpPost]`, or other appropriate HTTP method attributes, specifying the route for your endpoint.

5. **Define Request and Response Types**

   Define your `TRequest` and `TResponse` types in the same file as your endpoint class or in separate files as per your project structure.

6. **Test Your API Endpoint**

   Test your ASP.NET Core API endpoint. If you're using Swagger/OpenAPI, it should automatically document your endpoint based on the attributes provided.

#### Example Endpoint Class

Here's an example of a GET endpoint that returns a list of books using `Ardalis.ApiEndpoints`:

```csharp
public class ListBooksEndpoint : EndpointBaseSync
.WithoutRequest
.WithResult<IList<BookDto>>
{
    private readonly IRepository<Book> repository;
    private readonly IMapper mapper;

    public ListBooksEndpoint(
        IRepository<Book> repository,
        IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet("/books")]
    public override IList<BookDto> Handle()
    {
        var books = repository.ListAll();

        var bookDtos = books.Select(book => mapper.Map<BookDto>(book)).ToList();

        return bookDtos;
    }
}
```

This endpoint demonstrates listing books and uses the `HttpGet` annotation.

### Adding common endpoint groupings using Swagger

In a standard Web API controller, methods in the same class are grouped together in the Swagger UI. To add this same functionality for endpoints:

1. Install the Swashbuckle.AspNetCore.Annotations
``` bash
dotnet add package Swashbuckle.AspNetCore.Annotations
```
2. Add EnableAnnotations to the Swagger configuration in Startup.cs
``` csharp
services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.EnableAnnotations();
});
```
3. Add the following attribute to endpoint methods
``` csharp
[HttpPost("/authors")]
[SwaggerOperation(
    Summary = "Creates a new Author",
    Description = "Creates a new Author",
    OperationId = "Author_Create",
    Tags = new[] { "AuthorEndpoint" })
]
public override async Task<ActionResult<CreateAuthorResult>> HandleAsync([FromBody]CreateAuthorCommand request)
{
    var author = new Author();
    _mapper.Map(request, author);
    await _repository.AddAsync(author);

    var result = _mapper.Map<CreateAuthorResult>(author);
    return Ok(result);
}
```
Option to use service dependency injection instead of constructor
``` csharp
// File: sample/SampleEndpointApp/Endpoints/Authors/List.cs
public class List : BaseAsyncEndpoint
    .WithRequest<AuthorListRequest>
    .WithResponse<IList<AuthorListResult>>
{
    private readonly IAsyncRepository<Author> repository;
    private readonly IMapper mapper;

    public List(
        IAsyncRepository<Author> repository,
        IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }
    [HttpGet("/authors")]
    [SwaggerOperation(
        Summary = "List all Authors",
        Description = "List all Authors",
        OperationId = "Author_List",
        Tags = new[] { "AuthorEndpoint" })
    ]
    public override async Task<ActionResult<IList<AuthorListResult>>> HandleAsync(

        [FromQuery] AuthorListRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.PerPage == 0)
        {
            request.PerPage = 10;
        }
        if (request.Page == 0)
        {
            request.Page = 1;
        }
        var result = (await repository.ListAllAsync(request.PerPage, request.Page, cancellationToken))
            .Select(i => mapper.Map<AuthorListResult>(i));

        return Ok(result);
    }
}
```

Examples of the configuration can be found in the sample API project

### File Upload Example

[See Issue 170 for more details](https://github.com/ardalis/ApiEndpoints/issues/170)

```csharp
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace SampleEndpointApp.Endpoints.Authors;

public class File : EndpointBaseAsync
    .WithRequest<IFormFile>
    .WithResult<ActionResult<string[]>>
{
  /// <summary>
  /// Post author's photo or something
  /// </summary>
  [HttpPost("api/[namespace]/file")]
  public override async Task<ActionResult<string[]>> HandleAsync(
    IFormFile file,
    CancellationToken cancellationToken = default)
  {
    string filePath = Path.GetTempFileName();
    using (var fileStream = System.IO.File.Create(filePath))
    {
      await file.CopyToAsync(fileStream, cancellationToken);
    }

    return new[]
    {
      filePath,
      file.FileName,
      file.ContentType,
      file.ContentDisposition,
      file.Length.ToString()
    };
  }
}
```

## 4. Animated Screenshots

### Working with Endpoints, Requests, and Results in Visual Studio

![api-endpoints-2](https://user-images.githubusercontent.com/782127/107803375-7e509480-6d30-11eb-97bc-45da8396e8e8.gif)

## 5. Open Questions

Below are what I expect will be some common questions:

### How do I use shared routing conventions?

If you want to create a common route template for all or some subset of your Endpoints, simply create a EndpointBaseSync of your own that inherits from `Ardalis.Api.Endpoints.EndpointBaseSync` and add a `[Route]` attribute to it.

After refactoring to use the fluent generics pattern, there is no longer a way to use a base class for a default route. Instead, you should define your routes as constants which you can store in a central file or in each Request DTO (the sample shows this approach).

### Can I add more than one public routable method to an Endpoint class?

Technically, yes. But **don't do that**. If you really want that, you should just use a Controller.

### How can I bind parameters from multiple locations to my model?

To do this, you'll need to decorate the properties of your model with the proper route attributes:

``` csharp
public class NewArticleRequest
{
    [FromRoute(Name = "username")] public string Username { get; set; }
    [FromRoute(Name ="category")] public string Category { get; set; }

    [FromBody] public Article Article { get; set; }
}
```

Then, it's very important to include `[FromRoute]` in the method declaration in your endpoint using that model:

``` csharp
public override Task<ActionResult> HandleAsync([FromRoute] NewArticleRequest request)
```

Note the `[Route("/article")]` and `[HttpPost("{username}/{category}")]` lines below. These lines form the route string used in the `NewArticleRequest` class above.

``` csharp
[Route("/article")]
public class Post : BaseAsyncEndpoint
    .WithRequest<NewArticleRequest>
    .WithoutResponse
{
    [HttpPost("{username}/{category}")]
    [SwaggerOperation(
        Summary = "Submit a new article",
        Description = "Enables the submission of new articles",
        OperationId = "Article_Create",
        Tags = new[] {"Article"})
    ]
    public override Task<ActionResult> HandleAsync([FromRoute] NewArticleRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        //// your implementation
    }
}
```

For more information, take a look at [this discussion](https://github.com/ardalis/ApiEndpoints/issues/42) and [this issue](https://github.com/ardalis/ApiEndpoints/pull/50). Thank you to @garywoodfine and @matt-lethargic.

### How can I return a File result from an ApiEndpoint?

There's an example in the [sample app](https://github.com/ardalis/ApiEndpoints/blob/main/sample/SampleEndpointApp/Endpoints/Authors/ListJsonFile.cs) that shows how to set this up and return a File actionresult. For the base type, just use the `WithoutResponse` option and in the endpoint handler return `File()`.

### How can I use model binding to pull values from multiple places, like `[FromRoute]`, `[FromBody]`, etc.?
#172
The base endpoints only expose a single model type which is used on the Handle method, so you can't easily add additional parameters to the Handle method. However, you can put as many properties on the associated Request DTO as you want, and model binding allows you to set the same attributes per property as you would have set per parameter on the action method. See [Model Binding Docs](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding) and [discussion here in issue 42](https://github.com/ardalis/ApiEndpoints/issues/42)

### How can I use streaming from server to client?
There's an example in the [sample app](https://github.com/ardalis/ApiEndpoints/blob/main/sample/SampleEndpointApp/Endpoints/Authors/Stream.cs) that shows how to set this up and return an `IAsyncEnumerable<T>`. For the base type, just use the `WithAsyncEnumerableResult<T>` option and in the endpoint handler yeld return after awaiting your async code.

> Note: streaming with IAsyncEnumerable does not work within Swagger Ui. Use curl to test this functionality
> ``` bash
> curl -X "GET" "https://localhost:44338/api/Authors/stream" -H "accept: text/plain" 
## 6. Roadmap

The following are some things I'd like to add to the project/package.

### Item Template

Visual Studio and/or CLI item templates would make it much easier to create Endpoints and their associated models, with the correct naming so they're linked in the IDE.

### Route Conventions

One thing that Controllers do have is built-in support in the framework to use their name in routes (e.g. "`[controller]/{id?}`"). Currently in the sample app routes are hard-coded strings. It would be nice if there were an easy way to use a convention based on foldername or namespace or something (using foldername would align with how Razor Pages routing works).

## 7. Related Articles

- [Moving from Controllers and Actions to Endpoints](https://ardalis.com/moving-from-controllers-and-actions-to-endpoints-with-mediatr)
- [Decoupling Controllers with ApiEndpoints](https://betweentwobrackets.dev/posts/2020/09/decoupling-controllers-with-apiendpoints/)
- [Fluent Generics](https://tyrrrz.me/blog/fluent-generics)

## 8. Videos and Podcasts

- [Clean up your .NET Controllers with API Endpoints by Nick Chapsas](https://www.youtube.com/watch?v=SDu0MA6TmuM&ab_channel=NickChapsas)
- [The .NET Docs Show - Controllers are Dinosaurs and the Case for API Endpoints](https://www.youtube.com/watch?v=9oroj2TmxBs&ab_channel=dotNET)
- [.NET Rocks ASP.NET Core API Endpoints with Steve Smith](https://www.dotnetrocks.com/default.aspx?ShowNum=1695)

## 9. Related / Similar Projects

- [SimpleEndpoints](https://github.com/dasiths/SimpleEndpoints)
- [FunctionMonkey](https://github.com/JamesRandall/FunctionMonkey) A similar approach for Azure Functions.
- [https://github.com/Kahbazi/MediatR.AspNetCore.Endpoints](https://github.com/Kahbazi/MediatR.AspNetCore.Endpoints) A similar approach using MediatR and middleware.
- [Voyager](https://github.com/smithgeek/voyager) A similar approach using MediatR that works for ASP.NET core and Azure Functions.

## 10. Projects Using ApiEndpoints

If you're using them or find one not in this list, feel free to add it here via a pull request!

- [CleanArchitecture](https://github.com/ardalis/CleanArchitecture): A solution template for ASP.NET 3.x solutions using Clean Architecture.
- [PayrollProcessor](https://github.com/KyleMcMaster/payroll-processor): A smorgasbord of modern .NET tech written with functional and asynchronous patterns.
- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb): Sample ASP.NET Core reference application, powered by Microsoft

## 11. Success Stories and Testimonials

- [Developing APIs using HTTP Endpoints (Gary Woodfine)](https://garywoodfine.com/developing-apis-using-http-endpoints/)

> "I have implemented in my team your API endpoint solution and I must tell you that was a pretty good investment! in particular how maintainable and testable the solution became!"

[Nuno Santos](https://www.linkedin.com/in/nuno-santos-sd/)
