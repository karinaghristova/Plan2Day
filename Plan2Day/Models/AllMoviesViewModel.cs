using Plan2Day.Core.Models.Movies;

namespace Plan2Day.Models
{
    public class AllMoviesViewModel
    {
        public IEnumerable<MovieListViewModel> Movies { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
