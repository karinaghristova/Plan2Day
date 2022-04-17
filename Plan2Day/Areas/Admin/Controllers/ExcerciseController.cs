using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Infrastructure.Data.Identity;

namespace Plan2Day.Areas.Admin.Controllers
{
    public class ExcerciseController : Controller
    {
        private readonly IExerciseService exerciseService;

        private readonly UserManager<ApplicationUser> userManager;

        public ExcerciseController(IExerciseService exerciseService,
            UserManager<ApplicationUser> userManager)
        {
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }


        public async Task<IActionResult> ManageExercises(int sortingOrder = 0)
        {
            var exercises = await exerciseService.GetAllExercises();

            switch (sortingOrder)
            {
                case 0: return View(exercises.OrderBy(e => e.Name));
                case 1: return View(exercises.OrderBy(e => e.TargetMuscle));
                case 2: return View(exercises.OrderBy(e => e.Equipment));
                case 3: return View(exercises.OrderBy(e => e.MechanicsType));
                case 4: return View(exercises.OrderBy(e => e.Level));
                default:
                    return View(exercises);
            }

        }

        public async Task<IActionResult> DeleteExcercise(string exerciceId)
        {
            try
            {
                var result = await exerciseService.DeleteExercise(exerciceId);
                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully deleted excercise.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Excercise could not be deleted";
                }
                return RedirectToAction("AllExcercises");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
