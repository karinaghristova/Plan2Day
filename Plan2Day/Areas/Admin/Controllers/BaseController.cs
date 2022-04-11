using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;

namespace Plan2Day.Areas.Admin.Controllers
{
    [Authorize(Roles = UserConstant.Roles.Administrator)]
    [Area("Admin")]
    public class BaseController : Controller
    {

    }
}
