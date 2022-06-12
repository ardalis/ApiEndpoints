---
layout: default
title: ThenInclude
nav_order: 6
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# ThenInclude

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)
- [EF6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

The `ThenInclude` feature is used to indicate to the ORM that a related property of a previously `Include`d property should be returned with a query result.

## Example

Below is a specification that loads a Company entity along with its collection of Stores, *then* each Store's collection of Products.

```csharp
public class CompanyByIdWithStoresAndProducts : Specification<Company>, ISingleResultSpecification
{
  public CompanyByIdWithStoresAndProducts(int id)
  {
    Query.Where(company => company.Id == id)
      .Include(x => x.Stores)
        .ThenInclude(x => x.Products)
  }
}
```
