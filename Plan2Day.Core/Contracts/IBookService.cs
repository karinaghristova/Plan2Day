using Plan2Day.Core.Models.Books;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookListViewModel>> GetAllBooks();

        Task<IEnumerable<BookListViewModel>> GetAllReadBooks(string userId);

        Task<IEnumerable<BookListViewModel>> GetAllWantToReadBooks(string userId);

        Task<IEnumerable<BookListViewModel>> GetAllCurrentlyReadingBooks(string userId);

        Task<Book> GetBookByIdAsync(string id);
        Task<BookListViewModel> GetBookDetails(string id);

        Task<IEnumerable<BookGenreListViewModel>> GetAllGenresForBook(string id);

        Task<BookEditViewModel> EditBook(string id);

        Task<bool> UpdateBook(BookEditViewModel model);

        Task<bool> DeleteBook(string id);

        Task<bool> AddBookToCurrentlyReading(string userId, string bookId);

        Task<bool> AddBookToWantToRead(string userId, string bookId);

        Task<bool> MarkBookAsRead(string userId, string bookId);

        Task<bool> RemoveBookFromList(string userId, string movieId);
    }
}
