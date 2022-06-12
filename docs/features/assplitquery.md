---
layout: default
title: AsSplitQuery
nav_order: 3
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# AsSplitQuery

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)

[EF Core 5 introduced support for split queries](https://docs.microsoft.com/ef/core/querying/single-split-queries#split-queries-1) which will perform separate queries rather than complex joins when returning data from multiple tables. A single query result with data from many tables may result in a "cartesian explosion" of duplicate data across many columns and rows.

> EF allows you to specify that a given LINQ query should be split into multiple SQL queries. Instead of JOINs, split queries generate an additional SQL query for each included collection navigation.

## Example

Below is a specification that uses `AsSplitQuery` in order to generate several separate queries rather than a large join across the Company, Store, and Product tables:

```csharp
public class CompanyByIdAsSplitQuery : Specification<Company>, ISingleResultSpecification
{
  public CompanyByIdAsSplitQuery(int id)
  {
    Query.Where(company => company.Id == id)
      .Include(x => x.Stores)
      .ThenInclude(x => x.Products)
      .AsSplitQuery();
  }
}
```
