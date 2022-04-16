using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Movies;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;

namespace Plan2Day.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IApplicationDbRepository repo;

        public MovieService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> AddMovieToWatchList(string userId, string movieId)
        {
            bool added = false;
            var movie = await GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (movie != null && user != null)
            {
                var userMovie = new UserMovie()
                {
                    MovieId = movie.Id,
                    ApplicationUserId = user.Id,
                    MovieStatus = MovieConstants.WantToWatch
                };

                //To check if that movie and user are already present in the db
                var uMovie = await repo.All<UserMovie>()
                    .Where(um => um.MovieId == movie.Id)
                    .ToListAsync();

                var mov = uMovie.FirstOrDefault();

                if (mov != null)
                {
                    mov.MovieStatus = MovieConstants.WantToWatch;
                    repo.Update<UserMovie>(mov);
                }
                else
                {
                    await repo.AddAsync<UserMovie>(userMovie);
                }
                await repo.SaveChangesAsync();
                added = true;
            }

            return added;
        }

        public async Task<bool> MarkMovieAsWatched(string userId, string movieId)
        {
            bool marked = false;
            var movie = await GetMovieByIdAsync(movieId);

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (movie != null && user != null)
            {
                var userMovie = new UserMovie()
                {
                    MovieId = movie.Id,
                    ApplicationUserId = user.Id,
                    MovieStatus = MovieConstants.Watched
                };

                //To check if that movie and user are already present in the db and just change the status
                var uMovie = await repo.All<UserMovie>()
                    .Where(um => um.MovieId == movie.Id)
                    .ToListAsync();

                var mov = uMovie.FirstOrDefault();

                if (mov != null)
                {
                    mov.MovieStatus = MovieConstants.Watched;
                    repo.Update<UserMovie>(mov);
                }
                else
                {
                    await repo.AddAsync<UserMovie>(userMovie);
                }
                await repo.SaveChangesAsync();
                marked = true;
            }

            return marked;
        }

        public async Task<bool> DeleteMovie(string id)
        {
            bool deleted = false;
            var movie = await GetMovieByIdAsync(id);
            if (movie != null)
            {
                await repo.DeleteAsync<Movie>(movie.Id);
                await repo.SaveChangesAsync();
                deleted = true;
            }

            return deleted;
        }

        public async Task<MovieEditViewModel> EditMovie(string id)
        {
            var movie = await GetMovieByIdAsync(id);

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }

            return new MovieEditViewModel()
            {
                Id = movie.Id.ToString(),
                Title = movie.Title,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
                Runtime = movie.Runtime,
                Plot = movie.Plot,
            };
        }

        public async Task<IEnumerable<MovieGenreListViewModel>> GetAllGenresForMovie(string id)
        {
            var movie = await GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }

            return await repo.All<MovieGenre>()
                .Include(mg => mg.Movies)
                .Where(mg => mg.Movies.Contains(movie))
                .Select(mg => new MovieGenreListViewModel
                {
                    Id = mg.Id.ToString(),
                    Name = mg.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MovieListViewModel>> GetAllMovies()
        {
            return await repo.All<Movie>()
                .Include(m => m.Genres)
                .Select(m => new MovieListViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    Year = m.Year,
                    Runtime = m.Runtime,
                    Plot = m.Plot,
                    Genres = m.Genres
                }).ToListAsync();
        }

        public async Task<IEnumerable<MovieListViewModel>> GetAllWantToWatchMovies(string userId)
        {
            return await repo.All<UserMovie>()
                .Where(um => um.MovieStatus == MovieConstants.WantToWatch && um.ApplicationUserId == userId)
                .Include(um => um.Movie)
                .Select(m => new MovieListViewModel()
                {
                    Id = m.MovieId.ToString(),
                    Title = m.Movie.Title,
                    ImageUrl = m.Movie.ImageUrl,
                    Year = m.Movie.Year,
                    Runtime = m.Movie.Runtime,
                    Plot = m.Movie.Plot,
                    Genres = m.Movie.Genres
                }).ToListAsync();
        }

        public async Task<IEnumerable<MovieListViewModel>> GetAllWatchedMovies(string userId)
        {
            return await repo.All<UserMovie>()
                .Where(um => um.MovieStatus == MovieConstants.Watched && um.ApplicationUserId == userId)
                .Include(um => um.Movie)
                .Select(m => new MovieListViewModel()
                {
                    Id = m.MovieId.ToString(),
                    Title = m.Movie.Title,
                    ImageUrl = m.Movie.ImageUrl,
                    Year = m.Movie.Year,
                    Runtime = m.Movie.Runtime,
                    Plot = m.Movie.Plot,
                    Genres = m.Movie.Genres
                }).ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(string id)
        {
            var movie = await repo.GetByIdAsync<Movie>(new Guid(id));

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }
            return movie;
        }

        public async Task<bool> UpdateMovie(MovieEditViewModel model)
        {
            bool result = false;
            var movie = await repo.GetByIdAsync<Movie>(model.Id);

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }

            movie.Title = model.Title;
            movie.ImageUrl = model.ImageUrl;
            movie.Year = model.Year;
            movie.Runtime = model.Runtime;
            movie.Plot = model.Plot;

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<MovieListViewModel> GetMovieDetails(string id)
        {
            var movies = await repo.All<Movie>()
                .Include(m => m.Genres)
                .Select(m => new MovieListViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    Year = m.Year,
                    Runtime = m.Runtime,
                    Plot = m.Plot,
                    Genres = m.Genres
                }).ToListAsync();

            var movie = movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                throw new Exception("Movie could not be found");
            }

            return movie;
        }

        public async Task<bool> RemoveMovieFromList(string userId, string movieId)
        {
            bool removed = false;

            var movie = await GetMovieByIdAsync(movieId);
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);


            if (movie == null || user == null)
            {
                throw new Exception("Movie could not be found");
            }

            var userMovies = repo.All<UserMovie>()
                .Where(um => um.MovieId == movie.Id && um.ApplicationUserId == user.Id);
            var userMovie = await userMovies.FirstOrDefaultAsync();

            if (userMovie == null)
            {
                throw new Exception("Movie could not be found");
            }

            repo.Delete<UserMovie>(userMovie);
            await repo.SaveChangesAsync();
            removed = true;

            return removed;
        }
    }
}
