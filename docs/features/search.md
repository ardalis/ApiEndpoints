---
layout: default
title: Search
nav_order: 7
has_children: false
parent: ORM-Specific Features
grand_parent: Features
---

# Search

Compatible with:

- [EF Core](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)
- [EF6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

The `Search` extension filters the query source by applying an 'SQL LIKE' operation to it. The parameters for `Search` include the *Selector*, which is the property/column the LIKE should be applied against, and the *SearchTerm*, the value to use with the LIKE. Any wildcards (`%`) must be included in the SearchTerm.

## Example

The following example demonstrates how to use the `Search` feature:

```csharp
public class CustomerSpec : Specification<Customer>
{
  public CustomerSpec(CustomerFilter filter)
  {
    // other criteria omitted

    if (!string.IsNullOrEmpty(filter.Address))
    {
      Query
        .Search(x => x.Address, "%" + filter.Address + "%");
    }
  }
}
```
