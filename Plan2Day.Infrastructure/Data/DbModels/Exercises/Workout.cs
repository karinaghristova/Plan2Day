using Plan2Day.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.DbModels.Exercises
{
    public class Workout
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    }
}
