using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Books
{
    public class BookGenreListViewModel
    {
        public Guid Id { get; set; } = new Guid();

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

    }
}
