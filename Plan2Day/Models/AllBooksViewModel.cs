using Plan2Day.Core.Models.Books;

namespace Plan2Day.Models
{
    public class AllBooksViewModel
    {
        public IEnumerable<BookListViewModel> Books { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
