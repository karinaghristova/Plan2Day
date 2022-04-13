using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Plan2Day.Infrastructure.Data.DbModels.Activities;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.DbModels.Shopping;
using Plan2Day.Infrastructure.Data.Identity;

namespace Plan2Day.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<UserMovie> UserMovies { get; set; }


        //Exercises
        public DbSet<TargetMuscle> TargetMuscles { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<MechanicsType> MechanicsTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        //Shopping
        DbSet<Item> Items { get; set; }
        DbSet<ShoppingList> ShoppingLists { get; set; }

        //Activity
        DbSet<Category> Categories { get; set; }
        DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserMovie>().HasKey(k => new { k.ApplicationUserId, k.MovieId });
        }

    }
}