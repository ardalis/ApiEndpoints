---
layout: default
title: Evaluate
nav_order: 8
has_children: false
parent: Base Features
grand_parent: Features
---

# Evaluate

Apply a specification to an in memory collection.

## Example

First, a Specification can be defined to filter a given type. In this case, a specification that filters strings using a Contains clause.

```csharp
public class StringsWhereValueContainsSpec : Specification<string>
{
    public StringsWhereValueContainsSpec(string filter)
    {
        Query.Where(s => s.Contains(filter));
    }
}
```

You can apply the Specification above to an in memory collection using the `Evaluate` method. This method takes an `IEnumerable<T>` as a parameter representing the collection to apply the specification. A brief example is demonstrated below.

```csharp
var trainingResources = new[]
{
    "Articles",
    "Blogs",
    "Documentation",
    "Pluralsight",
};

var specification = new StringsWhereValueContainsSpec("ti");

var results = specification.Evaluate(trainingResources);
```

The result of `Evaluate` should be a collection of strings containing "Articles" and "Documentation". For additional information on `Evaluate` refer to the [Specifications with In Memory Collections](../usage/use-specification-inmemory-collection.md) guide.
