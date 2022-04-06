using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Movies;

namespace Plan2Day.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Books
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }

        //Movies
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        //Exercises
        public DbSet<TargetMuscle> TargetMuscles { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<MechanicsType> MechanicsTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }

    }
}