using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IApplicationDbRepository repo;


        public ExerciseService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> AddExerciseToWorkout(string exerciseId, string workoutId)
        {
            var added = false;

            var workout = await repo.GetByIdAsync<Workout>(Guid.Parse(workoutId));
            var exercise = await GetExerciseByIdAsync(exerciseId);


            if (exercise == null || workout == null)
            {
                throw new Exception("Exercise could not be added to workout.");
            }

            workout.Exercises.Add(exercise);
            repo.Update<Workout>(workout);
            await repo.SaveChangesAsync();
            added = true;

            return added;
        }

        public async Task<bool> ChangeEquipment(string exId, string equipmentId)
        {
            bool result = false;
            var exercise = await GetExerciseByIdAsync(exId);
            var equipment = await GetEquipmentByIdAsync(equipmentId);

            exercise.EquipmentId = equipment.Id;
            repo.Update<Exercise>(exercise);

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> ChangeLevel(string exId, string levelId)
        {
            bool result = false;
            var exercise = await GetExerciseByIdAsync(exId);
            var level = await GetLevelByIdAsync(levelId);

            exercise.LevelId = level.Id;
            repo.Update<Exercise>(exercise);

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> ChangeMechanicsType(string exId, string mechanicsTypeId)
        {
            bool result = false;
            var exercise = await GetExerciseByIdAsync(exId);
            var mechanicsType = await GetEquipmentByIdAsync(mechanicsTypeId);

            exercise.MechanicsTypeId = mechanicsType.Id;
            repo.Update<Exercise>(exercise);

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> ChangeTargetMuscle(string exId, string targetMuscleId)
        {
            bool result = false;
            var exercise = await GetExerciseByIdAsync(exId);
            var targetMuscle = await GetTargetMuscleByIdAsync(targetMuscleId);

            exercise.EquipmentId = targetMuscle.Id;
            repo.Update<Exercise>(exercise);

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<bool> CreateExercise(string name, string targetMuscleId, string equipmentId, string mechanicsTypeId, string levelId)
        {
            bool created = false;

            var targetMuscle = await GetTargetMuscleByIdAsync(targetMuscleId);
            var equipment = await GetEquipmentByIdAsync(equipmentId);
            var mechanicsType = await GetMechanicsTypeByIdAsync(mechanicsTypeId);
            var level = await GetLevelByIdAsync(levelId);


            var exercise = new Exercise
            {
                Name = name,
                TargetMuscleId = targetMuscle.Id,
                EquipmentId = equipment.Id,
                MechanicsTypeId = mechanicsType.Id,
                LevelId = level.Id
            };

            await repo.AddAsync<Exercise>(exercise);
            created = true;

            return created;
        }

        public async Task<bool> DeleteExercise(string exId)
        {
            bool deleted = false;
            var exercise = await GetExerciseByIdAsync(exId);

            if (exercise != null)
            {
                await repo.DeleteAsync<Exercise>(exercise.Id);
                await repo.SaveChangesAsync();
                deleted = true;
            }

            return deleted;
        }

        public async Task<IEnumerable<EquipmentViewModel>> GetAllEquipments()
        {
            return await repo.All<Equipment>()
                .Select(l => new EquipmentViewModel()
                {
                    Id = l.Id.ToString(),
                    Name = l.Name
                }).ToListAsync();
        }

        public async Task<IEnumerable<ExerciseListViewModel>> GetAllExercises()
        {
            return await repo.All<Exercise>()
                .Include(e => e.TargetMuscle)
                .Include(e => e.Equipment)
                .Include(e => e.MechanicsType)
                .Include(e => e.Level)
                .Select(e => new ExerciseListViewModel()
                {
                    Id = e.Id.ToString(),
                    Name = e.Name,
                    TargetMuscle = e.TargetMuscle.Name,
                    Equipment = e.Equipment.Name,
                    MechanicsType = e.MechanicsType.Name,
                    Level = e.Level.Name
                }).ToListAsync();
        }

        public async Task<IEnumerable<LevelViewModel>> GetAllLevels()
        {
            return await repo.All<Level>()
                .Select(l => new LevelViewModel()
                {
                    Id = l.Id.ToString(),
                    Name = l.Name
                }).ToListAsync();
        }

        public async Task<IEnumerable<MechanicsTypeViewModel>> GetAllMechanicsTypes()
        {
            return await repo.All<MechanicsType>()
                .Select(m => new MechanicsTypeViewModel()
                {
                    Id = m.Id.ToString(),
                    Name = m.Name
                }).ToListAsync();
        }

        public async Task<IEnumerable<TargetMuscleViewModel>> GetAllTargetMuscles()
        {
            return await repo.All<TargetMuscle>()
                .Select(t => new TargetMuscleViewModel()
                {
                    Id = t.Id.ToString(),
                    Name = t.Name
                }).ToListAsync();
        }

        public async Task<ExerciseListViewModel> GetDetailsForExercise(string id)
        {
            var exercise = await repo.All<Exercise>()
                .Where(e => e.Id.ToString() == id)
                .Include(e => e.TargetMuscle)
                .Include(e => e.Equipment)
                .Include(e => e.MechanicsType)
                .Include(e => e.Level)
                .FirstOrDefaultAsync();

            if (exercise == null)
            {
                throw new Exception("Excerise could not be found");
            }

            return new ExerciseListViewModel()
            {
                Id = exercise.Id.ToString(),
                Name = exercise.Name,
                TargetMuscle = exercise.TargetMuscle.Name,
                Equipment = exercise.Equipment.Name,
                MechanicsType = exercise.MechanicsType.Name,
                Level = exercise.Level.Name
            };
        }

        public async Task<Equipment> GetEquipmentByIdAsync(string equipmentId)
        {
            var equipment = await repo.GetByIdAsync<Equipment>(new Guid(equipmentId));

            if (equipment == null)
            {
                throw new Exception("Equipment could not be found");
            }

            return equipment;
        }

        public async Task<Exercise> GetExerciseByIdAsync(string id)
        {
            var exercise = await repo.GetByIdAsync<Exercise>(new Guid(id));

            if (exercise == null)
            {
                throw new Exception("Exercise could not be found");
            }

            return exercise;
        }

        public async Task<Level> GetLevelByIdAsync(string levelId)
        {
            var level = await repo.GetByIdAsync<Level>(new Guid(levelId));

            if (level == null)
            {
                throw new Exception("Level could not be found");
            }

            return level;
        }

        public async Task<MechanicsType> GetMechanicsTypeByIdAsync(string mechanicsTypeId)
        {
            var mechanicsType = await repo.GetByIdAsync<MechanicsType>(new Guid(mechanicsTypeId));

            if (mechanicsType == null)
            {
                throw new Exception("Mechanics Type could not be found");
            }

            return mechanicsType;
        }

        public async Task<TargetMuscle> GetTargetMuscleByIdAsync(string targetMuscleId)
        {
            var targetMuscle = await repo.GetByIdAsync<TargetMuscle>(new Guid(targetMuscleId));

            if (targetMuscle == null)
            {
                throw new Exception("Target muscle could not be found");
            }

            return targetMuscle;
        }
    }
}
