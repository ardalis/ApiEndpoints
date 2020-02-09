using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleEndpointApp.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEndpointApp.DataAccess.Config
{
    public class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired();

            builder.Property(e => e.PluralsightUrl)
                .IsRequired();

            builder.HasData(SeedData.Authors());
        }
    }
}
