---
layout: default
title: Select
nav_order: 7
has_children: false
parent: Base Features
grand_parent: Features
---

# Select

The `Select` feature defined in Specification behaves the same as `Select` in Linq, and it takes in `IEnumerable<TSource>` and `Func<TSource, TResult>` as its parameters.

`Select` is used to transform elements in a sequence into a new form. In Specification, `Select` is most commonly used to select a single property of each object in a list being queried. For example, the below expression could be used to retrieve only the name of each object:

```csharp
Query.Select(x => x.Name);
```

Since this query is now returning a different type, the type of `Name`, rather than of `x`, the base class of the Specification will need to reflect this. Instead of being a `Specification<T>`, the Specification will need to be a `Specification<T, TReturn>`:

```csharp
public class StoreNamesSpec : Specification<Store, string?>
{
    public StoreNamesSpec()
    {
        Query.Select(x => x.Name);
    }
}
```
