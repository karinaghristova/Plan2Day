using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Infrastructure.Data.Identity;

namespace Plan2Day.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
