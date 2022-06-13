---
layout: default
title: Caching
nav_order: 6
has_children: false
parent: Base Features
grand_parent: Features
---

# Caching

To implement caching using Specification, you will need to enable caching on your specification when it is defined:

```csharp
public class CustomerByNameWithStoresSpec : Specification<Customer>, ISingleResultSpecification
    {
        public CustomerByNameWithStoresSpec(string name)
        {
            Query.Where(x => x.Name == name)
                .Include(x => x.Stores)
                .EnableCache(nameof(CustomerByNameWithStoresSpec), name);
        }
    }
```

The `.EnableCache` method takes in two parameters: the name of the specification and the parameters of the specification. It does not include any parameters to control how the cache should behave (e.g. absolute expiration date, expiration tokens, ...). However, one could create an extension method to the specification builder in order to add this information ([example](../extensions/extend-specification-builder.md)).

Implementing caching will also require infrastructure such as a CachedRepository, an example of which is given in [the sample](https://github.com/ardalis/Specification/blob/2605202df4d8e40fe388732db6d8f7a3754fcc2b/sample/Ardalis.SampleApp.Infrastructure/Data/CachedCustomerRepository.cs#L13) on GitHub. The `EnableCache` method is used to inform the cache implementation that caching should be used, and to configure the `CacheKey` based on the arguments supplied.
