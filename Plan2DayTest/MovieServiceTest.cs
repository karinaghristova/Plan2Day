using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plan2DayTest
{
    public class MovieServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IMovieService service;
        private IApplicationDbRepository repo;

        private Guid movie1Guid = new Guid("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F");
        private Guid movie2Guid = new Guid("36C58319-20DA-43DB-3E3C-08DA1CE5EC9F");

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IMovieService, MovieService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicationDbRepository>();
            service = serviceProvider.GetService<IMovieService>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task GetAllMoviesMustReturnCorrectNumberOfMovies()
        {
            var allMovies = await service.GetAllMovies();

            Assert.AreEqual(allMovies.ToList().Count(), repo.All<Movie>().Count());
        }

        [Test]
        public async Task GetMovieByIdAsyncWorksCorrect()
        {
            var movie = await service.GetMovieByIdAsync("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F");

            Assert.DoesNotThrowAsync(async () => await service.GetMovieByIdAsync("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F"));
            Assert.AreEqual("TestMovie1", movie.Title);
            Assert.AreEqual("(2022)", movie.Year);
            Assert.AreEqual(90, movie.Runtime);
            Assert.AreEqual("First movie plot", movie.Plot);
        }

        [Test]
        public async Task GetMovieByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetMovieByIdAsync(null), "Movie could not be found");
        }

        [Test]
        public async Task AddMovieToWatchListWorksCorrect()
        {
            var userMoviesBefore = await repo.All<UserMovie>().ToListAsync();

            var result = service.AddMovieToWatchList("abcde", movie1Guid.ToString());

            var userMoviesAfter = await repo.All<UserMovie>().ToListAsync();

            var userMovie = repo.All<UserMovie>()
                .Where(um => um.MovieId == movie1Guid && um.ApplicationUserId == "abcde")
                .FirstOrDefault();

            Assert.AreEqual(userMoviesBefore.Count(), userMoviesAfter.Count() - 1);
            Assert.IsNotNull(userMovie);
            Assert.AreEqual(MovieConstants.WantToWatch, userMovie.MovieStatus);
        }

        [Test]
        public async Task AddMovieToWatchListThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.AddMovieToWatchList(null, null), "Movie could not be found");
            Assert.CatchAsync<ArgumentException>(async () => await service.AddMovieToWatchList("abcde", null), "Movie could not be found");
        }

        [Test]
        public async Task MarkMovieAsWatchedWorksCorrect()
        {
            var userMoviesBefore = await repo.All<UserMovie>().ToListAsync();

            var result = service.MarkMovieAsWatched("abcde", movie1Guid.ToString());

            var userMoviesAfter = await repo.All<UserMovie>().ToListAsync();

            var userMovie = repo.All<UserMovie>()
                .Where(um => um.MovieId == movie1Guid && um.ApplicationUserId == "abcde")
                .FirstOrDefault();

            Assert.AreEqual(userMoviesBefore.Count(), userMoviesAfter.Count() - 1);
            Assert.IsNotNull(userMovie);
            Assert.AreEqual(MovieConstants.Watched, userMovie.MovieStatus);
        }

        [Test]
        public async Task MarkMovieAsWatchedThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.MarkMovieAsWatched(null, null), "Movie could not be found");
            Assert.CatchAsync<ArgumentException>(async () => await service.MarkMovieAsWatched("abcde", null), "Movie could not be found");
        }

        [Test]
        public async Task DeleteMovieWorksCorrect()
        {
            var userMoviesBefore = await repo.All<Movie>().ToListAsync();

            var result = service.DeleteMovie(movie1Guid.ToString());

            var userMoviesAfter = await repo.All<Movie>().ToListAsync();


            Assert.AreEqual(userMoviesBefore.Count(), userMoviesAfter.Count() + 1);
        }

        [Test]
        public async Task GetAllGenresForMovieReturnCorrectData()
        {
            var genres = await service.GetAllGenresForMovie(movie1Guid.ToString());

            Assert.AreEqual(genres.ToList().Count(), 1);
        }

        [Test]
        public async Task GetAllGenresForMovieThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetAllGenresForMovie(null), "Movie could not be found");
        }

        [Test]
        public async Task GetAllWantToWatchMoviesReturnCorrectNumberOfMovies()
        {
            await service.AddMovieToWatchList("abcde", movie1Guid.ToString());

            var allWTWMovies = await service.GetAllWantToWatchMovies("abcde");

            var wtwMovies = repo.All<UserMovie>()
                .Where(um => um.ApplicationUserId == "abcde" && um.MovieStatus == MovieConstants.WantToWatch)
                .Count();

            Assert.AreEqual(allWTWMovies.ToList().Count(), wtwMovies);
        }

        [Test]
        public async Task GetAllWatchedMoviesMustReturnCorrectNumberOfMovies()
        {
            await service.MarkMovieAsWatched("abcde", movie2Guid.ToString());

            var allWMovies = await service.GetAllWatchedMovies("abcde");

            var wMovies = repo.All<UserMovie>()
                .Where(um => um.ApplicationUserId == "abcde" && um.MovieStatus == MovieConstants.Watched)
                .Count();

            Assert.AreEqual(allWMovies.ToList().Count(), wMovies);
        }

        [Test]
        public async Task RemoveMovieFromListDecreasesNumberOfUserMovies()
        {
            await service.MarkMovieAsWatched("abcde", movie2Guid.ToString());

            var wMoviesBefore = repo.All<UserMovie>()
                .Where(um => um.ApplicationUserId == "abcde")
                .Count();
            var result = await service.RemoveMovieFromList("abcde", movie2Guid.ToString());

            var wMoviesAfter = repo.All<UserMovie>()
               .Where(um => um.ApplicationUserId == "abcde")
               .Count();

            Assert.AreEqual(wMoviesBefore, wMoviesAfter + 1);
        }

        [TearDown]
        public void TearDown()
        {
            //Do not forget to dispose the dbContext
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var applicationUser = new ApplicationUser()
            {
                Id = "abcde",
                Email = "mytestuser@abv.bg",
                FirstName = "Pesho",
                LastName = "Ivanov",
            };

            await repo.AddAsync(applicationUser);

            var applicationUser2 = new ApplicationUser()
            {
                Id = "efghi",
                Email = "mysecondtestuser@abv.bg",
                FirstName = "Ivan",
                LastName = "Petrov",
            };
            await repo.AddAsync(applicationUser2);

            var genre1 = new MovieGenre()
            {
                Id = new System.Guid(),
                Name = "FirstGenre"
            };

            await repo.AddAsync(genre1);

            var genre2 = new MovieGenre()
            {
                Id = new System.Guid(),
                Name = "SecondGenre"
            };

            await repo.AddAsync(genre2);

            var movie1 = new Movie()
            {
                Id = movie1Guid,
                Title = "TestMovie1",
                ImageUrl = "https://imdb-api.com/images/original/MV5BMDdmMTBiNTYtMDIzNi00NGVlLWIzMDYtZTk3MTQ3NGQxZGEwXkEyXkFqcGdeQXVyMzMwOTU5MDk@._V1_Ratio0.6837_AL_.jpg",
                Year = "(2022)",
                Runtime = 90,
                Plot = "First movie plot"
            };

            movie1.Genres.Add(genre1);
            await repo.AddAsync(movie1);

            var movie2 = new Movie()
            {
                Id = movie2Guid,
                Title = "TestMovie2",
                ImageUrl = "https://imdb-api.com/images/original/MV5BMDdmMTBiNTYtMDIzNi00NGVlLWIzMDYtZTk3MTQ3NGQxZGEwXkEyXkFqcGdeQXVyMzMwOTU5MDk@._V1_Ratio0.6837_AL_.jpg",
                Year = "(2021)",
                Runtime = 190,
                Plot = "Second movie plot"
            };

            movie2.Genres.Add(genre2);
            await repo.AddAsync(movie2);


            await repo.SaveChangesAsync();
        }
    }
}