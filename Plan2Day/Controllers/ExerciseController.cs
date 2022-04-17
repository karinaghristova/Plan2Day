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

        public async Task<IActionResult> ShowAllExercises(string workoutId, int sortingOrder = 0)
        {
            var exercises = await exerciseService.GetAllExercises();

            switch (sortingOrder)
            {
                case 0: 
                    return View(new ShowAllExercisesViewModel { Exercises = exercises.OrderBy(e => e.Name), WorkoutId = workoutId });
                case 1: 
                    return View(new ShowAllExercisesViewModel { Exercises = exercises.OrderBy(e => e.TargetMuscle), WorkoutId = workoutId });
                case 2: 
                    return View(new ShowAllExercisesViewModel { Exercises = exercises.OrderBy(e => e.Equipment), WorkoutId = workoutId });
                case 3: 
                    return View(new ShowAllExercisesViewModel { Exercises = exercises.OrderBy(e => e.MechanicsType), WorkoutId = workoutId });
                case 4: 
                    return View(new ShowAllExercisesViewModel { Exercises = exercises.OrderBy(e => e.Level), WorkoutId = workoutId });
                default:
                    return View(new ShowAllExercisesViewModel { Exercises = exercises, WorkoutId = workoutId });
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

        public async Task<IActionResult> AddExerciseToWorkout(string exerciseId, string workoutId)
        {
            try
            {
                var result = await exerciseService.AddExerciseToWorkout(exerciseId, workoutId);

                return RedirectToAction("AllWorkouts", "Workout");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
