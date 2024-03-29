﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleEndpointApp.DataAccess;

namespace SampleEndpointApp.DataAccess.Migrations
{
  [DbContext(typeof(AppDbContext))]
  partial class AppDbContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "3.1.1");

      modelBuilder.Entity("SampleEndpointApp.DomainModel.Author", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("INTEGER");

            b.Property<string>("Name")
                      .IsRequired()
                      .HasColumnType("TEXT");

            b.Property<string>("PluralsightUrl")
                      .IsRequired()
                      .HasColumnType("TEXT");

            b.Property<string>("TwitterAlias")
                      .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Authors");

            b.HasData(
                      new
                  {
                    Id = 1,
                    Name = "Steve Smith",
                    PluralsightUrl = "",
                    TwitterAlias = "ardalis"
                  },
                      new
                  {
                    Id = 2,
                    Name = "Julie Lerman",
                    PluralsightUrl = "",
                    TwitterAlias = "julialerman"
                  });
          });
#pragma warning restore 612, 618
    }
  }
}
