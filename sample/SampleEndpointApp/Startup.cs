using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=database.sqlite")); // will be created in web project root

    services.AddControllers(options => options.UseNamespaceRouteToken());

    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleEndpointApp", Version = "v1" });
      c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SampleEndpointApp.xml"));
      c.UseApiEndpoints();
    });

    services.AddAutoMapper(typeof(Startup));

    services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleEndpointApp V1"));

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
