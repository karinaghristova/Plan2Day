using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data;
using Plan2Day.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IActivityService, ActivityService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}