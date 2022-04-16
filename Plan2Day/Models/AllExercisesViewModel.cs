using Plan2Day.Core.Models.Exercises;

namespace Plan2Day.Models
{
    public class AllExercisesViewModel
    {
        public IEnumerable<ExerciseListViewModel> Exercises { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
