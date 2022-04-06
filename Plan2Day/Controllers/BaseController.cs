using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Plan2Day.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

    }
}
