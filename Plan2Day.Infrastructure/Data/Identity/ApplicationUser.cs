using Microsoft.AspNetCore.Identity;
using Plan2Day.Infrastructure.Data.DbModels.Activities;
using Plan2Day.Infrastructure.Data.DbModels.Shopping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(200)]
        public string? FirstName { get; set; }

        [StringLength(200)]
        public string? LastName { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();

    }
}
