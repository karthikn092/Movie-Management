using System.Data.Entity;

namespace MovieManagement.Models
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() : base("MovieManagement")
        {
        }

        public DbSet<Actor> Actor { get; set; }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<Producer> Producer { get; set; }
    }
}