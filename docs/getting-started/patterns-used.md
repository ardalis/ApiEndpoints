---
layout: default
title: Patterns Used
parent: Getting Started
nav_order: 3
---

# Design Patterns Used

## Specification Pattern

In the [Specification Pattern](https://deviq.com/design-patterns/specification-pattern), specifications are used to define a query. Using a specification eliminates the need for scattering LINQ logic throughout the codebase, as the LINQ expressions can instead be encapsulated in the specification object. Additionally, using a specification to define the exact data required in a given query increases performance by ensuring only one query needs to be made at a time (as opposed to lazily loading each piece of data as it is required). As used in the [Ardalis.Specification package](https://www.nuget.org/packages/Ardalis.Specification), this pattern is used in conjunction with the [Repository Pattern](https://deviq.com/design-patterns/repository-pattern).

When used to define an object's state, the Specification Pattern can be used with the [Rules Engine Pattern](https://www.pluralsight.com/courses/c-sharp-design-patterns-rules-pattern) or the Factory Pattern. They can also be used to perform validation by specifying criteria that represent a valid instance of an object.

## Repository

By adding specifications to a repository implementation, a number of [SOLID principles](https://deviq.com/principles/solid) are better applied to the resulting solution. These typically only apply to repositories in larger and more complex applications, but if you're seeing your number or size of repositories grow in your applicaion, that's a code smell that indicates you may benefit from using specifications.

A common issue with repository is that custom queries require additional methods on repository interfaces and implementations. As new requirements arrive, more and more methods are appended to once-simple interfaces.

Opening and modifying the same types again and again is obviously breaking the [Open/Closed Principle](https://deviq.com/principles/open-closed-principle).

Adding more and more responsibilities (persistence, but also querying) to your repository types violates the [Single Responsibility Principle](https://deviq.com/principles/single-responsibility-principle).

Creating larger and larger repository interfaces violates the [Interface Segregation Principle](https://deviq.com/principles/interface-segregation).

Using a smaller, simpler repository interface that works with specifications solves all of these problems.
