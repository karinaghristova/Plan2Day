using Plan2Day.Infrastructure.Data.DbModels.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Movies
{
    public class MovieListViewModel
    {
        public Guid Id { get; set; } = new Guid();
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string? Year { get; set; }

        [Required]
        public int Runtime { get; set; }

        [StringLength(10000)]
        public string? Plot { get; set; }

        public ICollection<MovieGenre> Genres { get; set; } = new List<MovieGenre>();
    }
}
