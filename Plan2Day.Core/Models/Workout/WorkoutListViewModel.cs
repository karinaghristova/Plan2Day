using Plan2Day.Core.Models.Exercises;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Workout
{
    public class WorkoutListViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public ICollection<ExerciseListViewModel> Exercises { get; set; } = new List<ExerciseListViewModel>();
    }
}
