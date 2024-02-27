using Movies.Client.Models;

namespace Movies.Client.ApiServices;

public class MovieApiService : IMovieApiService
{
    public MovieApiService()
    {

    }
    public async Task<IEnumerable<Movie>> GetMovie()
    {
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
                } };
        return movies.ToList();

    }

    public Task<Movie> CreateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMovie(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Movie> GetMovie(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> UpdateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }
}
