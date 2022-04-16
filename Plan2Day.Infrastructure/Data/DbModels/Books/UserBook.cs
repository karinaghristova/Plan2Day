using Plan2Day.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Infrastructure.Data.DbModels.Books
{
    public class UserBook
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string BookStatus { get; set; }
    }
}
