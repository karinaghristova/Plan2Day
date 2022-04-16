using Microsoft.EntityFrameworkCore;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Books;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan2Day.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IApplicationDbRepository repo;

        public BookService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<bool> MarkBookAsRead(string userId, string bookId)
        {
            bool marked = false;
            var book = await GetBookByIdAsync(bookId);

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (book != null && user != null)
            {
                var userBook = new UserBook()
                {
                    BookId = book.Id,
                    ApplicationUserId = user.Id,
                    BookStatus = BookConstants.Read
                };

                var uBooks = await repo.All<UserBook>()
                    .Where(ub => ub.BookId == book.Id)
                    .ToListAsync();

                var bk = uBooks.FirstOrDefault();

                if (bk != null)
                {
                    bk.BookStatus = BookConstants.Read;
                    repo.Update<UserBook>(bk);
                }
                else
                {
                    await repo.AddAsync<UserBook>(userBook);
                }

                await repo.SaveChangesAsync();
                marked = true;
            }

            return marked;
        }

        public async Task<bool> AddBookToCurrentlyReading(string userId, string bookId)
        {
            bool marked = false;
            var book = await GetBookByIdAsync(bookId);

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (book != null && user != null)
            {
                var userBook = new UserBook()
                {
                    BookId = book.Id,
                    ApplicationUserId = user.Id,
                    BookStatus = BookConstants.Reading
                };

                var uBooks = await repo.All<UserBook>()
                    .Where(ub => ub.BookId == book.Id)
                    .ToListAsync();

                var bk = uBooks.FirstOrDefault();

                if (bk != null)
                {
                    bk.BookStatus = BookConstants.Reading;
                    repo.Update<UserBook>(bk);
                }
                else
                {
                    await repo.AddAsync<UserBook>(userBook);
                }

                await repo.SaveChangesAsync();
                marked = true;
            }

            return marked;
        }

        public async Task<bool> AddBookToWantToRead(string userId, string bookId)
        {
            bool marked = false;
            var book = await GetBookByIdAsync(bookId);

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            if (book != null && user != null)
            {
                var userBook = new UserBook()
                {
                    BookId = book.Id,
                    ApplicationUserId = user.Id,
                    BookStatus = BookConstants.WantToRead
                };

                var uBooks = await repo.All<UserBook>()
                    .Where(ub => ub.BookId == book.Id)
                    .ToListAsync();

                var bk = uBooks.FirstOrDefault();

                if (bk != null)
                {
                    bk.BookStatus = BookConstants.WantToRead;
                    repo.Update<UserBook>(bk);
                }
                else
                {
                    await repo.AddAsync<UserBook>(userBook);
                }

                await repo.SaveChangesAsync();
                marked = true;
            }

            return marked;
        }

        public async Task<bool> DeleteBook(string id)
        {
            bool deleted = false;
            var book = await GetBookByIdAsync(id);

            if (book != null)
            {
                await repo.DeleteAsync<Book>(book.Id);
                await repo.SaveChangesAsync();
                deleted = true;
            }

            return deleted;
        }

        public async Task<BookEditViewModel> EditBook(string id)
        {
            var book = await GetBookByIdAsync(id);

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            return new BookEditViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                ImageUrl = book.ImageUrl,
                Author = book.Author,
                Isbn13 = book.Isbn13,
                Pages = book.Pages,
                Year = book.Year,
                Description = book.Description
            };
        }

        public async Task<IEnumerable<BookListViewModel>> GetAllBooks()
        {
            return await repo.All<Book>()
                 .Include(b => b.Genres)
                 .Select(b => new BookListViewModel()
                 {
                     Id = b.Id,
                     Title = b.Title,
                     ImageUrl = b.ImageUrl,
                     Author = b.Author,
                     Isbn13 = b.Isbn13,
                     Pages = b.Pages,
                     Year = b.Year,
                     Description = b.Description
                 }).ToListAsync();
        }

        public async Task<IEnumerable<BookGenreListViewModel>> GetAllGenresForBook(string id)
        {
            var book = await GetBookByIdAsync(id);

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            return await repo.All<BookGenre>()
                .Include(bg => bg.Books)
                .Where(bg => bg.Books.Contains(book))
                .Select(bg => new BookGenreListViewModel()
                {
                    Id = bg.Id,
                    Name = bg.Name
                })
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(string id)
        {
            var book = await repo.GetByIdAsync<Book>(new Guid(id));

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            return book;
        }

        public async Task<bool> UpdateBook(BookEditViewModel model)
        {
            bool result = false;
            var book = await repo.GetByIdAsync<Book>(model.Id);


            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            book.Title = model.Title;
            book.ImageUrl = model.ImageUrl;
            book.Author = model.Author;
            book.Isbn13 = model.Isbn13;
            book.Pages = model.Pages;
            book.Year = model.Year;
            book.Description = model.Description;

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<IEnumerable<BookListViewModel>> GetAllReadBooks(string userId)
        {
            return await repo.All<UserBook>()
                .Where(ub => ub.ApplicationUserId == userId && ub.BookStatus == BookConstants.Read)
                .Include(ub => ub.Book)
                .Select(b => new BookListViewModel()
                {
                    Id = b.Book.Id,
                    Title = b.Book.Title,
                    ImageUrl = b.Book.ImageUrl,
                    Author = b.Book.Author,
                    Isbn13 = b.Book.Isbn13,
                    Pages = b.Book.Pages,
                    Year = b.Book.Year,
                    Description = b.Book.Description,
                }).ToListAsync();
        }

        public async Task<IEnumerable<BookListViewModel>> GetAllWantToReadBooks(string userId)
        {
            return await repo.All<UserBook>()
                .Where(ub => ub.ApplicationUserId == userId && ub.BookStatus == BookConstants.WantToRead)
                .Include(ub => ub.Book)
                .Select(b => new BookListViewModel()
                {
                    Id = b.Book.Id,
                    Title = b.Book.Title,
                    ImageUrl = b.Book.ImageUrl,
                    Author = b.Book.Author,
                    Isbn13 = b.Book.Isbn13,
                    Pages = b.Book.Pages,
                    Year = b.Book.Year,
                    Description = b.Book.Description,
                }).ToListAsync();
        }

        public async Task<IEnumerable<BookListViewModel>> GetAllCurrentlyReadingBooks(string userId)
        {
            return await repo.All<UserBook>()
                .Where(ub => ub.ApplicationUserId == userId && ub.BookStatus == BookConstants.Reading)
                .Include(ub => ub.Book)
                .Select(b => new BookListViewModel()
                {
                    Id = b.Book.Id,
                    Title = b.Book.Title,
                    ImageUrl = b.Book.ImageUrl,
                    Author = b.Book.Author,
                    Isbn13 = b.Book.Isbn13,
                    Pages = b.Book.Pages,
                    Year = b.Book.Year,
                    Description = b.Book.Description,
                }).ToListAsync();
        }

        public async Task<BookListViewModel> GetBookDetails(string id)
        {
            var books = await repo.All<Book>()
                .Include(b => b.Genres)
                .Select(b => new BookListViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    Author = b.Author,
                    Isbn13 = b.Isbn13,
                    Pages = b.Pages,
                    Year = b.Year,
                    Description = b.Description
                }).ToListAsync();

            var book = books.FirstOrDefault(b => b.Id == new Guid(id));

            if (book == null)
            {
                throw new Exception("Book could not be found");
            }

            return book;
        }

        public async Task<bool> RemoveBookFromList(string userId, string bookId)
        {
            bool removed = false;

            var book = await GetBookByIdAsync(bookId);
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);


            if (book == null || user == null)
            {
                throw new Exception("Book could not be found");
            }

            var userBooks = repo.All<UserBook>()
                .Where(ub => ub.BookId == book.Id && ub.ApplicationUserId == user.Id);
            var userBook = await userBooks.FirstOrDefaultAsync();

            if (userBook == null)
            {
                throw new Exception("Book could not be found");
            }

            repo.Delete<UserBook>(userBook);
            await repo.SaveChangesAsync();
            removed = true;

            return removed;
        }
    }
}
