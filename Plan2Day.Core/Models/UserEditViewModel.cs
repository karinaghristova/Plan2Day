using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }
    }
}
