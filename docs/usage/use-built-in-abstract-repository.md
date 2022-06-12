---
layout: default
title: How to use the Built In Abstract Repository
parent: Usage
nav_order: 5
---

# How to use the Built In Abstract Repository

## Introduction

Specifications shine when combined with the [Repository Pattern](https://deviq.com/design-patterns/repository-pattern). Get started using the one included in this package by following these steps. This example builds off the steps described in the [Quick Start Guide](../getting-started/quick-start-guide.md).

To use the abstract generic repository provided in this library, first define a repository class that inherits from `RepositoryBase<T>` in the Infrastructure or data access layer of your project. An example of this is provided in the sample web application in the [Specification repo](https://github.com/ardalis/Specification/blob/main/sample/Ardalis.SampleApp.Infrastructure/Data/MyRepository.cs). By inheriting from this base class, the generic repository class can now be used with any Entity supported by the provided DbContext. It also inherits many useful methods typically defined on a Repository without having to define them for each Entity type. This allows access to typical CRUD actions like `Add`, `Get`, `Update`, `Delete`, and `List` with minimal configuration and less duplicate code to maintain.

```csharp
public class YourRepository<T> : RepositoryBase<T> where T : class
{
    private readonly YourDbContext dbContext;

    public YourRepository(YourDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    // Not required to implement anything. Add additional functionalities if required.
}
```

It is important to remember to register this generic repository as a service with the application's Dependency Injection provider.

```csharp
services.AddScoped(typeof(YourRepository<>));
```

In the example below, two different services inject the same `YourRepository<T>` class but with different type arguments to perform similar actions. This allows for the creation of different services that can apply Specifications to collections of Entities without having to develop and maintain Repositories for each type.

```csharp
public class HeroByNameAndSuperPowerContainsFilterSpec : Specification<Hero>
{
    public HeroByNameAndSuperPowerContainsFilterSpec(string name, string superPower)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Query.Where(h => h.Name.Contains(name));
        }

        if (!string.IsNullOrEmpty(superPower))
        {
            Query.Where(h => h.SuperPower.Contains(superPower));
        }
    }
}

public class HeroService
{
    private readonly YourRepository<Hero> _heroRepository;

    public HeroService(YourRepository<Hero> heroRepository)
    {
        _heroRepository = heroRepository;
    }

    public async Task<List<Hero>> GetHeroesFilteredByNameAndSuperPower(string name, string superPower)
    {
        var spec = new HeroByNameAndSuperPowerContainsFilterSpec(name, superPower);

        return await _heroRepository.ListAsync(spec);
    }
}

public class CustomerByNameContainsFilterSpec : Specification<Customer>
{
    public CustomerByNameContainsFilterSpec(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Query.Where(c => c.Name.Contains(name));
        }
    }
}

public class CustomerService
{
    private readonly YourRepository<Customer> _customerRepository;

    public CustomerService(YourRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<Customer>> GetCustomersFilteredByName(string name)
    {
        var spec = new CustomerByNameContainsFilterSpec(name);

        return await _customerRepository.ListAsync(spec);
    }
}
```

## Features of `RepositoryBase<T>`

The section above introduced using `RepositoryBase<T>` to provide similar functionality across two entities and their services. This section aims to go into more detail about the methods made available by `RepositoryBase<T>` and provide some examples of their usages. Continuing with the HeroService example that contains a `private readonly YourRepository<Hero> _heroRepository` field, it is possible to create heroes as follows using the `AddAsync` method. The `SaveChangesAsync` method exposes the underlying DbContext method of the same name to persist changes to the database.

```csharp
public async Task<Hero> Create(string name, string superPower, bool isAlive, bool isAvenger)
{
    var hero = new Hero(name, superPower, isAlive, isAvenger);

    await _heroRepository.AddAsync(hero);

    await _heroRepository.SaveChangesAsync();

    return hero;
}
```

Now that a Hero has been created, it's possible to retrieve that Hero using either the Hero's Id or by using a Specification. Note that since the `HeroByNameSpec` returns a single Hero entity, the Specification inherits the interface `ISingleResultSpecification` which `GetBySpecAsync` uses to constrain the return type to a single Entity. In this case if more than one Hero was to exist for a given name, `GetBySpecAsync` performs a `FirstOrDefaultAsync` to return the first Hero of the result set.

```csharp
public class HeroByNameSpec : Specification<Hero>, ISingleResultSpecification
{
    public HeroByNameSpec(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Query.Where(h => h.Name == name);
        }
    }
}

public async Task<Hero> GetById(int id)
{
    return await _heroRepository.GetByIdAsync(id);
}

public async Task<Hero> GetByName(string name)
{
    var spec = new HeroByNameSpec(name);

    return await _heroRepository.GetBySpecAsync(spec);
}
```

Next, a Hero can be updated using `UpdateAsync`. `HeroService` defines a method `SetIsAlive` that takes an existing Hero and updates the IsAlive property.

```csharp
public async Task<Hero> SetIsAlive(int id, bool isAlive)
{
    var hero = await _heroRepository.GetByIdAsync(id);

    hero.IsAlive = isAlive;

    await _heroRepository.UpdateAsync(hero);

    await _heroRepository.SaveChangesAsync();

    return hero;
}
```

Removing Heroes can be done either by Hero using `DeleteAsync` or by collection using `DeleteRangeAsync`.

```csharp
public async Task Delete(Hero hero)
{
    await _heroRepository.DeleteAsync(hero);

    await _heroRepository.SaveChangesAsync();
}

public async Task DeleteRange(Hero[] heroes)
{
    await _heroRepository.DeleteRangeAsync(heroes);

    await _heroRepository.SaveChangesAsync();
}
```

The `RepositoryBase<T>` also provides two common Linq operations `CountAsync` and `AnyAsync`.

```csharp
public async Task SeedData(Hero[] heroes)
{
    // only seed if no Heroes exist
    if (await _heroRepository.AnyAsync())
    {
        return;
    }

    // alternatively
    if (await _heroRepository.CountAsync() > 0)
    {
        return;
    }

    foreach (var hero in heroes)
    {
        await _heroRepository.AddAsync(hero);
    }

    await _heroRepository.SaveChangesAsync();
}
```

The full HeroService implementation is shown below.

```csharp
public class HeroService
{
    private readonly YourRepository<Hero> _heroRepository;

    public HeroService(YourRepository<Hero> heroRepository)
    {
        _heroRepository = heroRepository;
    }

    public async Task<Hero> Create(string name, string superPower, bool isAlive, bool isAvenger)
    {
        var hero = new Hero(name, superPower, isAlive, isAvenger);

        await _heroRepository.AddAsync(hero);

        await _heroRepository.SaveChangesAsync();

        return hero;
    }

    public async Task Delete(Hero hero)
    {
        await _heroRepository.DeleteAsync(hero);

        await _heroRepository.SaveChangesAsync();
    }

    public async Task DeleteRange(List<Hero> heroes)
    {
        await _heroRepository.DeleteRangeAsync(heroes);

        await _heroRepository.SaveChangesAsync();
    }

    public async Task<Hero> GetById(int id)
    {
        return await _heroRepository.GetByIdAsync(id);
    }

    public async Task<Hero> GetByName(string name)
    {
        var spec = new HeroByNameSpec(name);

        return await _heroRepository.GetBySpecAsync(spec);
    }

    public async Task<List<Hero>> GetHeroesFilteredByNameAndSuperPower(string name, string superPower)
    {
        var spec = new HeroByNameAndSuperPowerFilterSpec(name, superPower);

        return await _heroRepository.ListAsync(spec);
    }

    public async Task<Hero> SetIsAlive(int id, bool isAlive)
    {
        var hero = await _heroRepository.GetByIdAsync(id);

        hero.IsAlive = isAlive;

        await _heroRepository.UpdateAsync(hero);

        await _heroRepository.SaveChangesAsync();

        return hero;
    }

    public async Task SeedData(Hero[] heroes)
    {
        // only seed if no Heroes exist
        if (!await _heroRepository.AnyAsync())
        {
            return;
        }

        // alternatively
        if (await _heroRepository.CountAsync() > 0)
        {
            return;
        }

        foreach (var hero in heroes)
        {
            await _heroRepository.AddAsync(hero);
        }

        await _heroRepository.SaveChangesAsync();
    }
}
```

## Putting it all together

The following sample program puts the methods described above together. Note the handling of dependencies is excluded for brevity.

```csharp
public async Task Run()
{
    var seedData = new[]
    {
        new Hero(
            name: "Batman",
            superPower: "Intelligence",
            isAlive: true,
            isAvenger: false),
        new Hero(
            name: "Iron Man",
            superPower: "Intelligence",
            isAlive: true,
            isAvenger: true),
        new Hero(
            name: "Spiderman",
            superPower: "Spidey Sense",
            isAlive: true,
            isAvenger: true),
    };

    await heroService.SeedData(seedData);

    var captainAmerica = await heroService.Create("Captain America", "Shield", true, true);

    var ironMan = await heroService.GetByName("Iron Man");

    var alsoIronMan = await heroService.GetById(ironMan.Id);

    await heroService.SetIsAlive(ironMan.Id, false);

    var shouldOnlyContainBatman = await heroService.GetHeroesFilteredByNameAndSuperPower("Bat", "Intel");

    await heroService.Delete(captainAmerica);

    var allRemainingHeroes = await heroService.GetHeroesFilteredByNameAndSuperPower("", "");

    await heroService.DeleteRange(allRemainingHeroes);
}
```

## Resources

An in-depth demo of a similar implementation of the Repository Pattern and `RepositoryBase<T>` can be found in the Repositories section of this [Pluralsight course](https://www.pluralsight.com/courses/domain-driven-design-fundamentals).
