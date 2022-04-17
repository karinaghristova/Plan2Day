using Plan2Day.Core.Models.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Contracts
{
    public interface IWorkoutService
    {
        Task<bool> CreateWorkout(string userId, string name);

        Task<IEnumerable<WorkoutListViewModel>> GetAllWorkouts(string userId);

        Task<bool> DeleteWorkout(string userId, string workoutId);
    }
}
