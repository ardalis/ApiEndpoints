# READ ME

## EF Migration Scripts

All of these commands should be run from the same folder as the .csproj file.

Make sure you have the global tool installed. If not, run this command:

```
dotnet tool install --global dotnet-ef
```

Once you have it, if you need to add migrations run this:

```
dotnet ef migrations add "Name" -o DataAccess/Migrations
```

Then to update the database (and reseed it) run this:

```
dotnet ef database update
```

## Duplicate Code in Endpoints

If the duplicate dependency code in the endpoints bothers you, you can avoid it easily by creating your own `AppEndpointBaseSync` classes that inherit from `EndpointBaseSync`. These can expose as properties any common dependencies like loggers, mappers, or generic repositories, and then use an IOC container's property injection feature to ensure they're always populated when the endpoint is created.