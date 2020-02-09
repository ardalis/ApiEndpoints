using SampleEndpointApp.DomainModel;
using System.Collections.Generic;

namespace SampleEndpointApp
{
    public static class SeedData
    {
        public static List<Author> Authors()
        {
            int id = 1;

            var authors = new List<Author>()
            {
                new Author
                {
                    Id = id++,
                    Name="Steve Smith",
                    PluralsightUrl="",
                    TwitterAlias="ardalis"
                },
                new Author
                {
                    Id = id++,
                    Name="Julie Lerman",
                    PluralsightUrl="",
                    TwitterAlias="julialerman"
                }
            };

            return authors;
        }
    }
}
