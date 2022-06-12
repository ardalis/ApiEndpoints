---
layout: default
title: OrderBy
nav_order: 2
has_children: false
parent: Base Features
grand_parent: Features
---

# OrderBy

The `OrderBy` feature defined in the Specification behaves the same as `OrderBy` in Linq, and it accepts `Expression<Func<TSource, TKey>>` expression as a parameter. The same is true for the related features `OrderByDescending`, `ThenBy`, and `ThenByDescending` described below.

`OrderBy`, as one might expect, is used to order the results of a query based on a key, defined by the lambda expression passed into `OrderBy`.

For example:

```csharp
Query.OrderBy(x => x.Name);
```

On the other hand, in order to order the results in the opposite order, one could instead use `OrderByDescending`, which works in the same manner as `OrderBy`:

```csharp
Query.OrderByDescending(x => x.Name);
```

Finally, `ThenBy` and `ThenByDescending` are also supported in the Specification and can be used to further refine the order of the results:

```csharp
Query.OrderByDescending(x => x.Name)
     .ThenByDescending(x => x.Id)
     .ThenBy(x => x.DateCreated);
```
