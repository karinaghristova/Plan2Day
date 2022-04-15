using Plan2Day.Core.Models.Movies;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieListViewModel>> GetAllMovies();

        Task<IEnumerable<MovieListViewModel>> GetAllWatchedMovies(string userId);

        Task<IEnumerable<MovieListViewModel>> GetAllWantToWatchMovies(string userId);


        Task<Movie> GetMovieById(string id);

        Task<IEnumerable<MovieGenreListViewModel>> GetAllGenresForMovie(string id);

        Task<MovieEditViewModel> EditMovie(string id);

        Task<bool> UpdateMovie(MovieEditViewModel model);

        Task<bool> DeleteMovie(string id);

        Task<bool> MarkMovieAsWatched(string userId, string movieId);

        Task<bool> AddMovieToWatchList(string userId, string movieId);

        //Task<bool> RemoveMovieFromWatched(string userId, string movieId);

        //Task<bool> RemoveMovieFromWantToWatch(string userId, string movieId);

    }
}
