---
layout: default
title: How to use Specifications with a DbContext
parent: Usage
nav_order: 3
---

# How to use Specifications with a DbContext

You can use Specifications to define queries that are executed directly using an EF6 or EF Core `DbContext`.
The following snippet defines a `Customer` entity and a sample `DbContext` which defines a `DbSet` of Customers.

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class SampleDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    // additional overrides intentionally left out
}
```

A specification can be applied directly to this `DbSet` using the `WithSpecification` extension method defined in `Ardalis.Specification.EntityFrameworkCore` package. Assuming a Specification is defined similar to the `ItemByIdSpec` described in [How to Create Specifications](./creating-specifications.md), the following code demonstrates putting these pieces together.

```csharp
// handling of IDisposable DbContext intentionally left out

int id = 1;

var specification = new CustomerByIdSpec(id);

var customer = dbContext.Customers
    .WithSpecification(specification)
    .FirstOrDefault();
```

Note that the `WithSpecification` extension method exposes an `IQueryable<T>` so additional extension methods maybe be applied after the specification. Some examples can be found below.

```csharp
bool isFound = dbContext.Customers.WithSpecification(specification).Any();

int customerCount = dbContext.Customers.WithSpecification(specification).Count();

var customers = dbContext.Customers.WithSpecification(specification).ToList();
```
