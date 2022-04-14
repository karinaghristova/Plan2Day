using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Movies;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IApplicationDbRepository repo;

        public MovieService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> DeleteMovie(string id)
        {
            bool deleted = false;
            var movie = await repo.GetByIdAsync<Movie>(id);
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
            var movie = await repo.GetByIdAsync<Movie>(id);
            return new MovieEditViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
                Runtime = movie.Runtime,
                Plot = movie.Plot,
            };
        }

        public async Task<IEnumerable<MovieGenreListViewModel>> GetAllGenresForMovie(string id)
        {
            var movie = await repo.GetByIdAsync<Movie>(id);
            return await repo.All<MovieGenre>()
                .Include(mg => mg.Movies)
                .Where(mg => mg.Movies.Contains(movie))
                .Select(mg => new MovieGenreListViewModel
                {
                    Id = mg.Id,
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
                    Id = m.Id,
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    Year = m .Year,
                    Runtime = m.Runtime,
                    Plot = m.Plot,
                    Genres = m.Genres
                }).ToListAsync();
        }

        public async Task<Movie> GetMovieById(string id)
        {
            return await repo.GetByIdAsync<Movie>(id);
        }

        public async Task<bool> UpdateMovie(MovieEditViewModel model)
        {
            bool result = false;
            var movie = await repo.GetByIdAsync<Movie>(model.Id);

            if (movie != null)
            {
                movie.Title = model.Title;
                movie.ImageUrl = model.ImageUrl;
                movie.Year = model.Year;
                movie.Runtime = model.Runtime;
                movie.Plot = model.Plot;

                await repo.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
