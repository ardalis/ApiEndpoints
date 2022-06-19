---
layout: default
title: Include
nav_order: 5
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# Include

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)
- [EF6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

The `Include` feature is used to indicate to the ORM that a related navigation property should be returned along with the base record being queried. It is used to expand the amount of related data being returned with an entity, providing [eager loading of related data](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager).

**Note**: [*Lazy-loading* is not recommended in web-based .NET applications](https://ardalis.com/avoid-lazy-loading-entities-in-asp-net-applications/).

## Example

Below is a specification that loads a Company entity along with its collection of Stores.

```csharp
public class CompanyByIdWithStores : Specification<Company>, ISingleResultSpecification
{
  public CompanyByIdWithStores(int id)
  {
    Query.Where(company => company.Id == id)
      .Include(x => x.Stores)
  }
}
```
