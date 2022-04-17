using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Exercises;
using Plan2Day.Core.Models.Workout;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IApplicationDbRepository repo;

        public WorkoutService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> CreateWorkout(string userId, string name)
        {
            bool created = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (user == null)
            {
                throw new Exception("User not found. Workout cannot be created");
            }

            var workout = new Workout
            {
                Name = name,
                ApplicationUserId = user.Id
            };

            await repo.AddAsync(workout);

            user.Workouts.Add(workout);

            repo.Update(user);
            await repo.SaveChangesAsync();
            created = true;

            return created;
        }

        public async Task<bool> DeleteWorkout(string userId, string workoutId)
        {
            bool deleted = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);
            var workout = await repo.GetByIdAsync<Workout>(Guid.Parse(workoutId));


            if (user == null || workout == null)
            {
                throw new Exception("User or workout not found. Workout cannot be deleted");
            }

            if (user.Workouts.Contains(workout))
            {
                await repo.DeleteAsync<Workout>(workout.Id);
                await repo.SaveChangesAsync();

                deleted = true;
            }

            return deleted;
        }

        public async Task<IEnumerable<WorkoutListViewModel>> GetAllWorkouts(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (user == null)
            {
                throw new Exception("User not found. Workout cannot be created");
            }

            return await repo.All<Workout>()
                .Where(w => w.ApplicationUserId == user.Id)
                .Include(w => w.Exercises)
                .Select(w => new WorkoutListViewModel()
                {
                    Id = w.Id.ToString(),
                    Name = w.Name,
                    Exercises = w.Exercises.Select(x => new ExerciseListViewModel 
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        TargetMuscle = x.TargetMuscle.Name,
                        Equipment = x.Equipment.Name,
                        MechanicsType = x.MechanicsType.Name,
                        Level = x.Level.Name
                    }).ToList()
                }).ToListAsync();
        }
    }
}
