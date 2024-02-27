using SecureMicroservice.Movies.Api.Models;

namespace SecureMicroservice.Movies.Api.Data;

public class MovieContextSeed
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (dbContext.Movies.Any())
            return;

        var movies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    Genre = "Drama",
                    Title = "The Shawshark Redemtion",
                    Rating = "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new DateTime(1993,5,5),
                    Owner = "Alice"
                },
                new Movie()
                {
                    Id = 2,
                    Genre = "Comic",
                    Title = "The  Redemtion",
                    Rating = "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new DateTime(1933,5,5),
                    Owner = "Alice"
                },
                new Movie()
                {
                    Id = 3,
                    Genre = "Action",
                    Title = " Shawshark Redemtion",
                    Rating = "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new DateTime(1943,5,5),
                    Owner = "Ben"
                },
                new Movie()
                {
                    Id = 4,
                    Genre = "Drama",
                    Title = "The Shawshark ",
                    Rating = "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = new DateTime(1993,5,5),
                    Owner = "Name"
                }
           };

        await dbContext.AddRangeAsync(movies);
        await dbContext.SaveChangesAsync();

    }
}
