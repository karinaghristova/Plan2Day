using System.ComponentModel.DataAnnotations;

namespace Plan2Day.Models
{
    public class UserWorkoutViewModel
    {
        [Required]
        public string userId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }
    }
}
