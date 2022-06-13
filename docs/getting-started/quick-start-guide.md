---
layout: default
title: Quick Start Guide
parent: Getting Started
nav_order: 2
---

# Ardalis.Specification Quick Start Guide

1. Install Nuget-Package(s)

   a. Always required: [Ardalis.Specification](https://www.nuget.org/packages/Ardalis.Specification/)

   b. If you want to use it with EF Core also install the package [Ardalis.Specification.EntityFrameworkCore](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)

   c. Alternatively, if you want to use it with EF6 also install the package [Ardalis.Specification.EntityFramework6](https://www.nuget.org/packages/Ardalis.Specification.EntityFramework6/)

2. Derive a Repository from `RepositoryBase<T>` in your Infrastructure project or layer where `YourDbContext` is defined.

   ```csharp
   public class YourRepository<T> : RepositoryBase<T> where T : class
   {
       private readonly YourDbContext _dbContext;

       public YourRepository(YourDbContext dbContext) : base(dbContext)
       {
           _dbContext = dbContext;
       }
   }
   ```

3. Create a first specification. It is good practice to define Specifications in the same layer as your domain entities.

   ```csharp
   public class CustomerByLastnameSpec : Specification<Customer>
   {
       public CustomerByLastnameSpec(string lastname)
       {
           Query.Where(c => c.Lastname == lastname);
       }
   }
   ```

4. Register your Repository as a service to the dependency injection provider of your choice.

   ```csharp
   services.AddScoped(typeof(YourRepository<>));
   ```

5. Bind it all together:

   ```csharp
   public class CustomerService {
       private readonly YourRepository<Customer> customerRepository;

       public CustomerService (YourRepository<Customer> customerRepository) {
           this.customerRepository = customerRepository;
       }

       public Task<List<Customer>> GetCustomersByLastname(string lastname) {
           return customerRepository.ListAsync(new CustomerByLastnameSpec(lastname));
       }
   }
   ```
