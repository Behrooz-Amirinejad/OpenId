using Microsoft.EntityFrameworkCore;
using SecureMicroservice.Movies.Api.Models;

namespace SecureMicroservice.Movies.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions option):base(option)
    {
        
    }

    public DbSet<Movie> Movies{ get; set; }
}
