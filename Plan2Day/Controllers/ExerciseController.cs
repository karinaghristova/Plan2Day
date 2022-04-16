using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Exercises;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Models;

namespace Plan2Day.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IExerciseService exerciseService;

        private readonly UserManager<ApplicationUser> userManager;

        public ExerciseController(IExerciseService exerciseService, 
            UserManager<ApplicationUser> userManager)
        {
            this.exerciseService = exerciseService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> AllExercises(int sortingOrder = 0)
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

        public async Task<IActionResult> ExcerciseDetails(string exerciseId)
        {
            try
            {
                var model = await exerciseService.GetDetailsForExercise(exerciseId);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
