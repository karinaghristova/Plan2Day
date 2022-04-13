using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.DbModels.Shopping
{
    public class ShoppingList
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
