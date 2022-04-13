using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.DbModels.Activities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
