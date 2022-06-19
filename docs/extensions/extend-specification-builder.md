---
layout: default
title: How to create your own specification builder
parent: Extensions
nav_order: 2
---


# How to add extensions to the specification builder

The specification builder from `Ardalis.Specification` is extensible by design. In fact, the methods you can use out of the box are implemented as extension methods themselves (check out the [source code](https://github.com/ardalis/Specification/blob/main/Specification/src/Ardalis.Specification/Builder/SpecificationBuilderExtensions.cs)). Your project might have requirements that cannot be satisfied by the existing toolset of course, or you might want to simplify repetitive code in several specification constructors. Whatever your case, enhancing the default builder is easy by creating your own extension methods.

So where do you start? A good practice is to write the thing you think you need. Say you'd like to use a builder method `WithCustomerIdAndName` that takes the `Id` and `Name` of a customer as parameters. Then just write it like so:

````csharp
Query.AsNoTracking()
    .WithCustomerIdAndName(1337, "John Doe");
````

From here you can inspect the return type of the builder method you chained it to (`AsNoTracking`), and create an extension method on that interface (it doesn't need to be chained of course -- working on `Query` itself is also valid). This will most likely be `ISpecificationBuilder<T>`, but in some cases it's an inherited inteface. The example below illustrates how extension methods on inherited interfaces allow the builder to offer specific methods in specific contexts.


## Example: Configure caching behaviour through specification builder extension method

In order to achieve this (note the `.WithTimeToLive` method):

````csharp
public class CustomerByNameWithStores : Specification<Customer>
{
    public CustomerByNameWithStores(string name)
    {
        Query.Where(x => x.Name == name)
            .EnableCache(nameof(CustomerByNameWithStoresSpec), name)
                // Can only be called after .EnableCache()
                .WithTimeToLive(TimeSpan.FromHours(1))
            .Include(x => x.Stores);
    }
}
````

We can create a simple extension method on the specification builder:

````csharp
public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> WithTimeToLive<T>(this ICacheSpecificationBuilder<T> @this, TimeSpan ttl)
        where T : class
    {
        // The .SetCacheTTL method is an extension method which is discussed below
        @this.Specification.SetCacheTTL(ttl);
        return @this;
    }
}
````

This extension method can only be called when chained after `EnableCache`. This is because `EnableCache` returns `ICacheSpecificationBuilder<T>` which inherits from `ISpecificationBuilder<T>`. Which is nice because it helps the IDE to give the right suggestions in the right place, and because it avoids confusing code as the `.WithTimeToLive` cannot be used without its *parent* `EnableCache` method.

The next thing we need to is use the TTL information in a repository. For example:

```csharp
public class Repository<T>
{
    private DbContext _ctx;
    private MemoryCache _cache;

    public List<T> List(ISpecification<T> spec)
    {
        var specificationResult = SpecificationEvaluator.Default.GetQuery(_ctx.Set<T>().AsQueryable(), spec);

        if (spec.CacheEnabled)
        {
            // The .GetCacheTTL method is an extension method which is discussed below
            var ttl = spec.GetCacheTTL();

            // Uses Microsoft's MemoryCache to cache the result
            _cache.GetOrCreate(spec.CacheKey, ce =>
            {
                ce.AbsoluteExpiration = DateTime.Now.Add(ttl);
                return specificationResult.ToList();
            });
        }
        else
        {
            return specificationResult.ToList();
        }
    }
}
```

Finally, we need to take care of some plumbing to implement the `.GetCacheTTL` and `.SetCacheTTL` methods that we've used in the example repository and builder extension.

````csharp
public static class SpecificationExtensions
{
    public static void SetCacheTTL<T>(this ISpecification<T> spec, TimeSpan timeToLive)
    {
        spec.Items["CacheTTL"] = timeToLive;
    }
    public static TimeSpan GetCacheTTL<T>(this ISpecification<T> spec)
    {
        spec.Items.TryGetValue("CacheTTL", out var ttl);
        return (ttl as TimeSpan?) ?? TimeSpan.MaxValue;
    }
}
````