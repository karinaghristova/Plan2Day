using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Models;

namespace Plan2Day.Controllers
{
    public class MovieController : Controller
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
    }
}
