---
layout: default
title: IgnoreQueryFilters
nav_order: 4
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# IgnoreQueryFilters

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)

The `IgnoreQueryFilters` feature is used to indicate to EF Core (it is not supported by EF 6) that it should ignore global query filters for this query. It simply passes along this call to the underlying [EF Core feature for disabling global filters](https://docs.microsoft.com/ef/core/querying/filters#disabling-filters).

## Example

The following specification implements the `IgnoreQueryFilters()` expression:

```csharp
public class CompanyByIdIgnoreQueryFilters : Specification<Company>, ISingleResultSpecification
{
  public CompanyByIdIgnoreQueryFilters(int id)
  {
    Query
      .Where(company => company.Id == id)
      .IgnoreQueryFilters();
  }
}
```
