using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Models.Books
{
    public class BookEditViewModel
    {
        public Guid Id { get; set; } = new Guid();

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(250)]
        public string Author { get; set; }

        [Required]
        [StringLength(13)]
        public string Isbn13 { get; set; }

        public int? Pages { get; set; }

        [Required]
        public int Year { get; set; }

        [StringLength(10000)]
        public string? Description { get; set; }

    }
}
