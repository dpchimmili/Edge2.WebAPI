using Microsoft.EntityFrameworkCore;
using Edge2.WebAPIs.Entities;


namespace Edge2.WebAPIs.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Movie> Movies { get; set; }

    }
}
