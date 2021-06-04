[![dotnet core - build](https://github.com/ardalis/ApiEndpoints/workflows/dotnet%20core%20-%20build/badge.svg)](https://github.com/ardalis/ApiEndpoints/actions?query=workflow%3A%22dotnet+core+-+build%22) 
[![Nuget](https://img.shields.io/nuget/v/Ardalis.ApiEndpoints)](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)
[![Nuget](https://img.shields.io/nuget/dt/Ardalis.ApiEndpoints)](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)

# ASP.NET Core API Endpoints

A project for supporting API Endpoints in ASP.NET Core web applications.

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Upgrade to 3.x Notes

For version 3.0 we implemented a new way to define the base classes using "fluent generics". You can [watch a video of what you need to know to apply them to your site here](https://www.youtube.com/watch?v=hKiuj0huEI4&ab_channel=Ardalis).

## Table of Contents

[1. Motivation](#1-motivation)

[2. Introducing ASP.NET Core API Endpoints](#2-introducing-aspnet-core-api-endpoints)

[3. Getting Started](#3-getting-started)

[4. Animated Screenshots](#4-animated-screenshots)

[5. Open Questions](#5-open-questions)

[6. Roadmap](#6-roadmap)

[7. Related Articles](#7-related-articles)

[8. Related / Similar Projects](#8-related--similar-projects)

[9. Projects Using ApiEndpoints](#9-projects-using-apiendpoints)

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

I'll look to add detailed documentation in the future but for now here's all you need to get started (you can also check the sample project):

1. Add the [Ardalis.ApiEndpoints NuGet package](https://www.nuget.org/packages/Ardalis.ApiEndpoints/) to your ASP.NET Core web project.
2. Create Endpoint classes by inheriting from either `BaseEndpoint<TRequest,TResponse>` (for endpoints that accept a model as input) or `BaseEndpoint<TResponse>` (for endpoints that simply return a response). For example, a POST endpoint that creates a resource and then returns the newly created record would use the version that includes both a Request and a Response. A GET endpoint that just returns a list of records and doesn't accept any arguments would use the second version.
3. Implement the base class's abstract `Handle()` method.
4. Make sure to add a `[HttpGet]` or similar attribute to your `Handle()` method, specifying its route.
5. Define your `TResponse` type in a file in the same folder as its corresponding endpoint (or in the same file if you prefer). 
6. Define your `TRequest` type (if any) just like the `TResponse` class.
7. Test your ASP.NET Core API Endpoint. If you're using Swagger/OpenAPI it should just work with it automatically.

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
    OperationId = "Author.Create",
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
// File: sample/SampleEndpointApp/AuthorEndpoints/List.cs
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
        OperationId = "Author.List",
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

## 4. Animated Screenshots

### Working with Endpoints, Requests, and Results in Visual Studio

![api-endpoints-2](https://user-images.githubusercontent.com/782127/107803375-7e509480-6d30-11eb-97bc-45da8396e8e8.gif)

## 5. Open Questions

Below are what I expect will be some common questions:

### How do I use shared routing conventions?

~~If you want to create a common route template for all or some subset of your Endpoints, simply create a BaseEndpoint of your own that inherits from `Ardalis.Api.Endpoints.BaseEndpoint` and add a `[Route]` attribute to it.~~

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
        OperationId = "B349A6C4-1198-4B53-B9BE-85232E06F16E",
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

## 6. Roadmap

The following are some things I'd like to add to the project/package.

### Item Template

Visual Studio and/or CLI item templates would make it much easier to create Endpoints and their associated models, with the correct naming so they're linked in the IDE.

### Route Conventions

One thing that Controllers do have is built-in support in the framework to use their name in routes (e.g. "/[controller]/{id?}"). Currently in the sample app routes are hard-coded strings. It would be nice if there were an easy way to use a convention based on foldername or namespace or something (using foldername would align with how Razor Pages routing works).

## 7. Related Articles

- [Moving from Controllers and Actions to Endpoints](https://ardalis.com/moving-from-controllers-and-actions-to-endpoints-with-mediatr)
- [Decoupling Controllers with ApiEndpoints](https://betweentwobrackets.dev/posts/2020/09/decoupling-controllers-with-apiendpoints/)
- [Fluent Generics](https://tyrrrz.me/blog/fluent-generics)

## 8. Related / Similar Projects

- [SimpleEndpoints](https://github.com/dasiths/SimpleEndpoints)
- [FunctionMonkey](https://github.com/JamesRandall/FunctionMonkey) A similar approach for Azure Functions.
- [https://github.com/Kahbazi/MediatR.AspNetCore.Endpoints](https://github.com/Kahbazi/MediatR.AspNetCore.Endpoints) A similar approach using MediatR and middleware.
- [Voyager](https://github.com/smithgeek/voyager) A similar approach using MediatR that works for ASP.NET core and Azure Functions.

## 9. Projects Using ApiEndpoints

If you're using them or find one not in this list, feel free to add it here via a pull request!

- [CleanArchitecture](https://github.com/ardalis/CleanArchitecture): A solution template for ASP.NET 3.x solutions using Clean Architecture.
- [PayrollProcessor](https://github.com/KyleMcMaster/payroll-processor): A smorgasbord of modern .NET tech written with functional and asynchronous patterns.
- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb): Sample ASP.NET Core reference application, powered by Microsoft

