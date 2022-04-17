using Plan2Day.Core.Models.Exercises;

namespace Plan2Day.Models
{
    public class ShowAllExercisesViewModel
    {
        public IEnumerable<ExerciseListViewModel> Exercises { get; set; }
        public string WorkoutId { get; set; }
    }
}
