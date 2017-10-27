using Microsoft.EntityFrameworkCore;

namespace MvcMovie.Infrastructure.Data
{
    public class MvcMovieContext : DbContext
    {
        //ira add to satisfy test
        public MvcMovieContext()
        {
        }
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Core.Models.Movie> Movie { get; set; }
    }
}
