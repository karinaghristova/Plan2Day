using Plan2Day.Core.Models.Workout;

namespace Plan2Day.Models
{
    public class AllWorkoutsViewModel
    {
        public IEnumerable<WorkoutListViewModel> Workouts { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
