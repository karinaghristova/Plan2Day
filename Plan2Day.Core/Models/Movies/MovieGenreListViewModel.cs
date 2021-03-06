using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Movies
{
    public class MovieGenreListViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }
    }
}
