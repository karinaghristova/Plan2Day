using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.DbModels.Exercises
{
    public class Exercise
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid TargetMuscleId{ get; set; }

        [ForeignKey(nameof(TargetMuscleId))]
        public TargetMuscle TargetMuscle { get; set; }

        [Required]
        public Guid EquipmentId { get; set; }

        [ForeignKey(nameof(EquipmentId))]
        public Equipment Equipment { get; set; }

        [Required]
        public Guid MechanicsTypeId { get; set; }

        [ForeignKey(nameof(MechanicsTypeId))]
        public MechanicsType MechanicsType { get; set; }

        [Required]
        public Guid LevelId { get; set; }

        [ForeignKey(nameof(LevelId))]
        public Level Level { get; set; }

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
