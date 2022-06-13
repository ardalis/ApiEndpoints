---
layout: default
title: How to Create Specifications
parent: Usage
nav_order: 1
---

# How to Create Specifications

## Basic Specification

A Specification class should inherit from `Specification<T>`, where `T` is the type being retrieved in the query:

```csharp
public class ItemByIdSpec : Specification<Item>
```

A Specification can take parameters in its constructor and use these parameters to make the appropriate query. Since the above class's name indicates that it will retrieve an `Item` *by id*, its constructor should take in an `id` parameter:

```csharp
public ItemByIdSpec(int Id)
```

In its constructor, the Specification should define a `Query` expression, using its parameter to retrieve the desired object:

```csharp
Query.Where(x => x.Id == Id);
```

Based on the above, the most basic specification should look something like this:

```csharp
public class ItemByIdSpec : Specification<Item>
{
    public ItemByIdSpec(int Id)
    {
        Query.Where(x => x.Id == Id);
    }
}
```

Finally: the Specification above should also implement the marker interface `ISingleResultSpecification`, which makes clear that this Specification will return only one result. Any "ById" Specification, and any other Specification intended to return only one result, should implement this interface to make clear that it returns a single result.

```csharp
public class ItemByIdSpec : Specification<Item>, ISingleResultSpecification
{
    public ItemByIdSpec(int Id)
    {
        Query.Where(x => x.Id == Id);
    }
}
```

## Advanced Specification

From here, additional operators can be used to further refine the Specification. These operators follow LINQ syntax and are described in more detail in the [Features](../features/index.md) section.
