---
layout: default
title: AsNoTrackingWithIdentityResolution
nav_order: 2
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# AsNoTrackingWithIdentityResolution

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)

The `AsNoTrackingWithIdentityResolution` feature applies this method to the resulting query executed by[EF Core](https://docs.microsoft.com/en-us/ef/core/change-tracking/identity-resolution#identity-resolution-and-queries). It is not supported by EF 6.

> No-tracking queries can be forced to perform identity resolution by using `AsNoTrackingWithIdentityResolution<TEntity>(IQueryable<TEntity>)`. The query will then keep track of returned instances (without tracking them in the normal way) and ensure no duplicates are created in the query results.

## Example

The following example shows how to add `AsNoTrackingWithIdentityResolution` to a specification:

```csharp
public class CustomerByNameReadOnlySpec : Specification<Customer>
{
  public CustomerByNameReadOnlySpec(string name)
  {
    Query.Where(x => x.Name == name)
      .AsNoTrackingWithIdentityResolution()
      .OrderBy(x => x.Name)
        .ThenByDescending(x => x.Address);
    }
}
```

**Note:** It's a good idea to note when specifications use `AsNoTracking` (or `AsNoTrackingWithIdentityResolution`) so that consumers of the specification will not attempt to modify and save entities returned by queries using the specification. The above specification adds `ReadOnly` to its name for this purpose.
