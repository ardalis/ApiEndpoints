---
layout: default
title: Overview
nav_order: 1
has_children: false
---
# Overview

The [Specification pattern](https://deviq.com/design-patterns/specification-pattern) encapsulates query logic in its own class, which helps classes follow the [Single Responsibility Principle](https://deviq.com/principles/single-responsibility-principle) (SRP) and promotes reuse of common queries. Specifications can be independently unit tested. When combined with the [Repository](https://deviq.com/design-patterns/repository-pattern) pattern, it can also help to keep it from growing with too many additional custom query methods. Specification is commonly used on projects that leverage [Domain-Driven Design](https://deviq.com/domain-driven-design/ddd-overview).

Since version 5, this package also supports applying specifications directly to EF Core `DbContext` instances.

## Benefits

The main benefits offered by the specification pattern in general and this package in particular are:

- Keep data access query logic in one place
- Keep data access query logic in the domain layer
- Reuse common queries throughout your application
- Provide good names to common queries to facilitate reuse and elevate language used to describe the app's behavior
- Eliminate common pain points of Repository pattern (hiding ORM data shaping features, requiring many custom query methods)

## Installing Ardalis.Specification

Install Ardalis.Specification from NuGet. The latest version is available here:

[https://www.nuget.org/packages/Ardalis.Specification/](https://www.nuget.org/packages/Ardalis.Specification/)

Alternately, add it to a project using this CLI command:

```powershell
dotnet add package Ardalis.Specification
```

## Getting Started

Read the [Getting Started section](getting-started/). You can also [review the sample application that is available in the GitHub repository](https://github.com/ardalis/Specification/tree/master/sample).

## Docs theme notes

This docs site is using the [Just the Docs theme](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/). Details on how to configure its metadata and navigation can be found [here](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/).
