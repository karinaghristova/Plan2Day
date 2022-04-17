using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Models;

namespace Plan2Day.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService workoutService;

        private readonly UserManager<ApplicationUser> userManager;

        public WorkoutController(IWorkoutService workoutService,
            UserManager<ApplicationUser> userManager)
        {
            this.workoutService = workoutService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> AllWorkouts(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var workouts = await workoutService.GetAllWorkouts(userId);
                int workoutsPerPage = PageConstants.PageSize20;
                int workoutsToSkip = page == 1 ? 0 : page * workoutsPerPage;

                return View(new AllWorkoutsViewModel
                {
                    Workouts = workouts.Skip(workoutsToSkip).Take(workoutsPerPage),
                    Paging = new PagingViewModel
                    {
                        CurrentPage = page,
                        TotalPages = (int)Math.Ceiling(workouts.Count() / (double)workoutsPerPage)
                    }
                });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
                throw;
            }

           
        }

        public async Task<IActionResult> CreateWorkout(string workoutName)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var model = new UserWorkoutViewModel()
            {
                userId = userId,
                Name = workoutName
            };

            return View(model);
        }

        public async Task<IActionResult> DeleteWorkout(string workoutId)
        {
            var userId = userManager.GetUserId(HttpContext.User);
            try
            {
                var result = await workoutService.DeleteWorkout(userId, workoutId);
                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully deleted workout.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Workout could not be deleted";
                }
                return RedirectToAction("AllWorkouts");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkout(UserWorkoutViewModel model)
        {
            try
            {
                await workoutService.CreateWorkout(model.userId, model.Name);
                return RedirectToAction("AllWorkouts");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
