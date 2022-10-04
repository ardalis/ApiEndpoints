---
layout: default
title: Overview
nav_order: 1
has_children: false
---
# Overview

To do: Change this to cover API Endpoints.

ASP.NET Core API Endpoints are essentially Razor Pages for APIs. They break apart bloated controllers and group the API models used by individual endpoints with the endpoint logic itself. They provide a simple way to have a single file for the logic and linked files for the model types.

When working with ASP.NET Core API Endpoints your project won't need any Controller classes. You can organize the Endpoints however you want. By feature. In a giant Endpoints folder. It doesn't matter - they'll work regardless of where you put them.

Most REST APIs have groups of endpoints for a given resource. In Controller-based projects you would have a controller per resource. When using API Endpoints you can simply create a folder per resource, just as you would use folders to group related pages in Razor Pages.

Instead of Model-View-Controller (MVC) the pattern becomes Request-EndPoint-Response(REPR). The REPR (reaper) pattern is much simpler and groups everything that has to do with a particular API endpoint together. It follows SOLID principles, in particular SRP and OCP. It also has all the benefits of feature folders and better follows the Common Closure Principle by grouping together things that change together.

## Installing Ardalis.ApiEndpoints

Install Ardalis.Specification from NuGet. The latest version is available here:

[https://www.nuget.org/packages/Ardalis.ApiEndpoints/](https://www.nuget.org/packages/Ardalis.ApiEndpoints/)

Alternately, add it to a project using this CLI command:

```powershell
dotnet add package Ardalis.ApiEndpoints
```

## Docs theme notes

This docs site is using the [Just the Docs theme](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/). Details on how to configure its metadata and navigation can be found [here](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/).
