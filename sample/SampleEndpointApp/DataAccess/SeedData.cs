using SampleEndpointApp.DomainModel;
using System.Collections.Generic;

namespace SampleEndpointApp.DataAccess
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
                    PluralsightUrl="https://www.pluralsight.com/authors/steve-smith",
                    TwitterAlias="ardalis"
                },
                new Author
                {
                    Id = id++,
                    Name="Julie Lerman",
                    PluralsightUrl="https://www.pluralsight.com/authors/julie-lerman",
                    TwitterAlias="julialerman"
                }
            };

            return authors;
        }
    }
}
