using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Contracts;

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


    }
}
