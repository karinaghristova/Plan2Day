using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Exercises
{
    public class ExerciseListViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string TargetMuscle { get; set; }

        [Required]
        [StringLength(250)]
        public string Equipment { get; set; }

        [Required]
        [StringLength(250)]
        public string MechanicsType { get; set; }

        [Required]
        [StringLength(250)]
        public string Level { get; set; }
    }
}
