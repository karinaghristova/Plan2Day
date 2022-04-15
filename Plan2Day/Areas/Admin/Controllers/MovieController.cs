using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Movies;
using Plan2Day.Models;

namespace Plan2Day.Areas.Admin.Controllers
{
    public class MovieController : BaseController
    {
        private IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageMovies(int page = 1)
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
                    TotalPages = (int)Math.Ceiling(movies.Count() / (double)moviesPerPage)
                }
            });
        }

        public async Task<IActionResult> DeleteMovie(string movieId)
        {
            try
            {
                var result = await movieService.DeleteMovie(movieId);
                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully deleted movie.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be deleted";
                }
                return RedirectToAction("ManageMovies");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> EditMovie(string movieId)
        {
            try
            {
                var model = await movieService.EditMovie(movieId);

                return View(model);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be edited.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(MovieEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await movieService.UpdateMovie(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Successfully edited movie.";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be edited.";
            }

            return View(model);
        }
    }
}
