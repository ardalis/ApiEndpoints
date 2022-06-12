---
layout: default
title: How to use Specifications with the Repository Pattern
parent: Usage
nav_order: 2
---

# How to use Specifications with the Repository Pattern

Specifications shine when combined with the [Repository Pattern](https://deviq.com/design-patterns/repository-pattern), a sample generic implementation of which is included in this NuGet package. For the purpose of this walkthrough, the repository can be thought of as a simple data access abstraction over a collection of entities. In this example, the entity for a `Hero`, the repository implementation, and its interface are defined below.

```csharp
public class Hero
{
    public string Name { get; set; }
    public string SuperPower { get; set; }
    public bool IsAlive { get; set; }
    public bool IsAvenger { get; set; }
}

public interface IHeroRepository
{
    List<Hero> GetAllHeroes();
}

public class HeroRepository : IHeroRepository
{
    private readonly HeroDbContext _dbContext;

    public HeroRepository(HeroDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Hero> GetAllHeroes()
    {
        return _dbContext.Heroes.ToList();
    }
}
```

It's possible to extend this existing repository to support Specifications by adding a parameter for the specification to the `GetAllHeroes` method and then modifying the repository to apply the query of the Specification to the underlying data store. A basic implementation of this using the default value for `SpecificationEvaluator` and no `PostProcessingAction` is as follows. For a deeper dive, it is worth looking into the internals of [this abstract class](https://github.com/ardalis/Specification/blob/main/Specification.EntityFrameworkCore/src/Ardalis.Specification.EntityFrameworkCore/RepositoryBaseOfT.cs). This example also depends on DbContext provided by Entity Framework, although any IQueryable should work in place of `_dbContext.Heroes`.

```csharp
public interface IHeroRepository
{
    List<Hero> GetAllHeroes(Specifcation<Hero> specification);
}

public class HeroRepository : IHeroRepository
{
    private readonly HeroDbContext _dbContext;

    public HeroRepository(HeroDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Hero> GetAllHeroes(ISpecification<Hero> specification)
    {
        var queryResult = SpecificationEvaluator.Default.GetQuery(
            query: _dbContext.Heroes.AsQueryable(),
            specification: specification);

        return queryResult.ToList();
    }
}
```

Now that the Hero repository supports Specifications, a Specification can be defined that filters by whether the Hero is alive and is an Avenger. Note any fields that are needed to filter the Heroes are passed to the Specification's constructor where the query logic should be implemented.

```csharp
public class HeroByIsAliveAndIsAvengerSpec : Specification<Hero>
{
    public HeroByIsAliveAndIsAvengerSpec(bool isAlive, bool isAvenger)
    {
        Query.Where(h => h.IsAlive == isAlive && h.IsAvenger == isAvenger);
    }
}
```

With the Specification and Repository defined, it is now possible to define a `GetHeroes` method that can take a `HeroRepository` as a parameter along with the filtering conditions and produce a filtered collection of heroes. Applying the Repository to the Specification is done using the `Evaluate` method on the Specification class which takes a `IEnumerable<T>` as a parameter. This should mirror the kind of methods typically found on Controllers or [Api Endpoints](https://github.com/ardalis/ApiEndpoints) where the IHeroRepository might be supplied via Dependency Injection to the class's constructor rather than passed as a parameter.

```csharp
public List<Hero> GetHeroes(IHeroRepository repository, bool isAlive, bool isAvenger)
{
    var specification = new HeroByIsAliveAndIsAvengerSpec(isAlive, isAvenger);

    return repository.GetAllHeroes(specification);
}
```

Suppose the data store behind the IHeroRepository has the following state and client code calls the `GetHeroes` as below. The result should be a collection containing only the Spider Man hero.

<div markdown="1">

| Name       | SuperPower   | IsAlive | IsAvenger |
| :--------- | :----------- | :------ | :-------- |
| Batman     | Intelligence | true    | false     |
| Iron Man   | Intelligence | false   | true      |
| Spider Man | Spidey Sense | true    | true      |

</div>

```csharp
var result = GetHeroes(repository: repository, isAlive: true, isAvenger: true);
```

## Further Reading

For more information on the Repository Pattern and the sample generic implementation included in this package, see the [How to use the Built In Abstract Repository](./use-built-in-abstract-repository.md) tutorial.
