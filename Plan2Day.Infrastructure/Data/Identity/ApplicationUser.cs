using Microsoft.AspNetCore.Identity;
using Plan2Day.Infrastructure.Data.DbModels.Activities;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using System.ComponentModel.DataAnnotations;

namespace Plan2Day.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(200)]
        public string? FirstName { get; set; }

        [StringLength(200)]
        public string? LastName { get; set; }

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
