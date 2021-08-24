using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }
    }
}
