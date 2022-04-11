using Microsoft.AspNetCore.Mvc;

namespace Plan2Day.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
