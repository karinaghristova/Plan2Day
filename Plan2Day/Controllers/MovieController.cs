using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Models;
using System.Security.Claims;

namespace Plan2Day.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        //private readonly ILogger<MovieController> logger;
        private readonly UserManager<ApplicationUser> userManager;


        public MovieController(IMovieService movieService, UserManager<ApplicationUser> userManager)
        {
            this.movieService = movieService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllMovies(int page = 1)
        {
            var movies = await movieService.GetAllMovies();
            int moviesPerPage = PageConstants.PageSize50;
            int moviesToSkip = page == 1 ? 0 : page * moviesPerPage;

            return View(new AllMoviesViewModel
            {
                Movies = movies.Skip(moviesToSkip).Take(moviesPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(movies.Count() / (double) moviesPerPage)
                }
            });
        }

        public async Task<IActionResult> WantToWatchMovies(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var movies = await movieService.GetAllWantToWatchMovies(userId);
            int moviesPerPage = PageConstants.PageSize50;
            int moviesToSkip = page == 1 ? 0 : page * moviesPerPage;

            return View(new AllMoviesViewModel
            {
                Movies = movies.Skip(moviesToSkip).Take(moviesPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(movies.Count() / (double)moviesPerPage)
                }
            });
        }

        public async Task<IActionResult> AddMovieToWantToWatch(string movieId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await movieService.AddMovieToWatchList(userId, movieId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully added movie to watch list.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be added to watch list.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be added to watch list.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("WantToWatchMovies");
        }

        public async Task<IActionResult> WatchedMovies(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var movies = await movieService.GetAllWatchedMovies(userId);
            int moviesPerPage = PageConstants.PageSize50;
            int moviesToSkip = page == 1 ? 0 : page * moviesPerPage;

            return View(new AllMoviesViewModel
            {
                Movies = movies.Skip(moviesToSkip).Take(moviesPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(movies.Count() / (double)moviesPerPage)
                }
            });
        }

        public async Task<IActionResult> AddMovieToWatched(string movieId)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            try
            {
                var result = await movieService.MarkMovieAsWatched(userId, movieId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully added movie to watched list.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be added to watched list";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be added to watched list.";
                return RedirectToAction("Index", "Home");
            }
            

            return RedirectToAction("WatchedMovies");
        }

        public async Task<IActionResult> RemoveMovieFromList(string movieId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await movieService.RemoveMovieFromList(userId, movieId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully removed movie";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("AllMovies");

        }

        public async Task<IActionResult> MovieDetails(string movieId)
        {
            try
            {
                var resultModel = await movieService.GetMovieDetails(movieId);
                ViewData[MessageConstant.SuccessMessage] = "Movie details successfully loaded.";
                return View(resultModel);

            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie details could not be loaded.";
                return RedirectToAction("AllMovies");
            }

        }
    }
}
