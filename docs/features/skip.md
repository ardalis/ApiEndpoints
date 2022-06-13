---
layout: default
title: Skip
nav_order: 3
has_children: false
parent: Base Features
grand_parent: Features
---

# Skip

The `Skip` feature defined in the Specification behaves the same as `Skip` in Linq, and it accepts an `int count` as a parameter.

`Skip` is used to skip a certain number of the results in a query, starting from the beginning. For example:

```csharp
int[] numbers = { 1, 3, 2, 5, 7, 4 };

IEnumerable<int> subsetOfNumbers = numbers.Skip(2);
```

Here, `subsetOfNumbers` would contain `{ 2, 5, 7, 4 }`.

Alternatively:

```csharp
int[] numbers = { 1, 3, 2, 5, 7, 4 };

IEnumerable<int> subsetOfNumbers = numbers.OrderBy(n => n).Skip(2);
```

Here, `subsetOfNumbers` would contain `{ 3, 4, 5, 7 }`.

`Skip` is commonly used in combination with [Take](take.md) to implement [Paging](paging.md), but as the above demonstrates, `Skip` can also be used on its own.
