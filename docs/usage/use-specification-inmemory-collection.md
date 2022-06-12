---
layout: default
title: How to use Specifications with In Memory Collections
parent: Usage
nav_order: 4
---

# How to use Specifications with In Memory Collections

You can use Specifications on collections of objects in memory. This approach can be convenient when retrieving data doesn't require querying a remote or out of process data store like a database. If the process does require querying external persistence, it is better to refer to the practices for using a Specification with a [Repository Pattern](./use-specification-repository-pattern.md) or [DbContext](./use-specification-dbcontext.md).

A Specification can be applied to an in memory collection using the `Evaluate` method. This method takes a single parameter of type `IEnumerable<T>` representing the collection on which the underlying query expression will be appled to. In this example, the `GetEnvironment` method of the `Example` class handles creating a Specification and applying it to the collect static collection of Environment entities.

```csharp
public class Environment
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class EnvironmentByNameSpec : Specification<Environment>
{
    public EnvironmentByNameSpec(string name)
    {
        Query.Where(h => h.Name == name);
    }
}

public static class Example
{
    private static readonly Environment[] Environments = new Environment[]
    {
        new()
        {
            Name = "DEV",
            Description = "this application's development environment"
        },
        new()
        {
            Name = "QA",
            Description = "this application's QA environment"
        },
        new()
        {
            Name = "PROD",
            Description = "this application's production environment"
        }
    };

    public static Environment GetEnvironment(string name)
    {
        var specification = new EnvironmentByNameSpec(name);

        var environment = specification.Evaluate(Environments)
            .Single();

        return environment;
    }
}
```
