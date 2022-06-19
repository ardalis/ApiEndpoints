---
layout: default
title: Paging
nav_order: 5
has_children: false
parent: Base Features
grand_parent: Features
---

# Paging

You can use [Skip](skip.md) and [Take](take.md) to implement paging with Specification.

## Example

A simple Specification with paging might look something like this:

```csharp
public class StoresByCompanyPaginatedSpec : Specification<Store>
{
    public StoresByCompanyPaginatedSpec(int companyId, int skip, int take)
    {
        Query.Where(x => x.CompanyId == companyId)
             .Skip(skip)
             .Take(take);
    }
}
```

*Find the most recent version of this Specification [here](https://github.com/ardalis/Specification/blob/master/ArdalisSpecification/tests/Ardalis.Specification.UnitTests/Fixture/Specs/StoresByCompanyPaginatedSpec.cs).*

## How paging should work

To implement paging, you should `Skip` `i * n` entries, where `i` is the index of the page you're on (starting from zero), and `n` is the number of entries per page. Then you should `Take` `n` entries. When paging through a set of data, each request must include the appropriate `Skip` and `Take` values for the page being requested.
