using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using Plan2Day.Infrastructure.Data.DbModels.Movies;

namespace Plan2Day.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
    }
}