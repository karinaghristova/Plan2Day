using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Contracts;

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

        public async Task<IActionResult> AllMovies()
        {
            var movies = await movieService.GetAllMovies();

            return View(movies);
        }
    }
}
