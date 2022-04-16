using Plan2Day.Core.Models.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Contracts
{
    public interface IExerciseService
    {
        Task<bool> CreateExercise(string name, string targetMuscleId, string equipmentId,
            string mechanicsTypeId, string levelId);
        Task<bool> ChangeTargetMuscle(string exId, string targetMuscleId);
        Task<bool> ChangeEquipment(string exId, string equipmentId);
        Task<bool> ChangeMechanicsType(string exId, string mechanicsTypeId);
        Task<bool> ChangeLevel(string exId, string levelId);
        Task<bool> DeleteExercise(string exId);

        Task<ExerciseListViewModel> GetDetailsForExercise(string id);
        Task<IEnumerable<ExerciseListViewModel>> GetAllExercises();
        Task<IEnumerable<TargetMuscleViewModel>> GetAllTargetMuscles();
        Task<IEnumerable<EquipmentViewModel>> GetAllEquipments();
        Task<IEnumerable<MechanicsTypeViewModel>> GetAllMechanicsTypes();
        Task<IEnumerable<LevelViewModel>> GetAllLevels();

        Task<Exercise> GetExerciseByIdAsync(string id);
        Task<TargetMuscle> GetTargetMuscleByIdAsync(string targetMuscleId);
        Task<Equipment> GetEquipmentByIdAsync(string equipmentId);
        Task<MechanicsType> GetMechanicsTypeByIdAsync(string mechanicsTypeId);
        Task<Level> GetLevelByIdAsync(string levelId);

    }
}
