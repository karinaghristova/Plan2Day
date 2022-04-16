using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data.DbModels.Books;
using Plan2Day.Infrastructure.Data.DbModels.Movies;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plan2DayTest
{
    public class BookServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IBookService service;
        private IApplicationDbRepository repo;

        private Guid book1Guid = new Guid("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F");
        private Guid book2Guid = new Guid("36C58319-20DA-43DB-3E3C-08DA1CE5EC9F");

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IBookService, BookService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicationDbRepository>();
            service = serviceProvider.GetService<IBookService>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task GetAllBooksMustReturnCorrectNumberOfUsers()
        {
            var allBooks = await service.GetAllBooks();

            Assert.AreEqual(allBooks.ToList().Count(), repo.All<Book>().Count());
        }

        [Test]
        public async Task GetBookByIdAsyncWorksCorrect()
        {
            var book = await service.GetBookByIdAsync(book1Guid.ToString());

            Assert.DoesNotThrowAsync(async () => await service.GetBookByIdAsync(book1Guid.ToString()));
            Assert.AreEqual("Test Book 1", book.Title);
            Assert.AreEqual("Author 1", book.Author);
            Assert.AreEqual("9780062892409", book.Isbn13);
            Assert.AreEqual(100, book.Pages);
            Assert.AreEqual(2022, book.Year);
            Assert.AreEqual("First book description", book.Description);
        }

        [Test]
        public async Task GetBookByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetBookByIdAsync(null), "Movie could not be found");
        }

        [Test]
        public async Task AddBookToCurrentlyReadingWorksCorrect()
        {
            var userBooksBefore = await repo.All<UserBook>().ToListAsync();

            var result = service.AddBookToCurrentlyReading("abcde", book1Guid.ToString());

            var userBooksAfter = await repo.All<UserBook>().ToListAsync();

            var userBook = repo.All<UserBook>()
                .Where(ub => ub.BookId == book1Guid && ub.ApplicationUserId == "abcde")
                .FirstOrDefault();

            Assert.AreEqual(userBooksBefore.Count(), userBooksAfter.Count() - 1);
            Assert.IsNotNull(userBook);
            Assert.AreEqual(BookConstants.Reading, userBook.BookStatus);
        }

        [Test]
        public async Task AddBookToCurrentlyReadingThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.AddBookToCurrentlyReading(null, null), "Book could not be found");
            Assert.CatchAsync<ArgumentException>(async () => await service.AddBookToCurrentlyReading("abcde", null), "Book could not be found");
        }

        [Test]
        public async Task AddBookToWantToReadWorksCorrect()
        {
            var userBooksBefore = await repo.All<UserBook>().ToListAsync();

            var result = service.AddBookToWantToRead("abcde", book1Guid.ToString());

            var userBooksAfter = await repo.All<UserBook>().ToListAsync();

            var userBook = repo.All<UserBook>()
                .Where(ub => ub.BookId == book1Guid && ub.ApplicationUserId == "abcde")
                .FirstOrDefault();

            Assert.AreEqual(userBooksBefore.Count(), userBooksAfter.Count() - 1);
            Assert.IsNotNull(userBook);
            Assert.AreEqual(BookConstants.WantToRead, userBook.BookStatus);
        }

        [Test]
        public async Task AddBookToWantToReadThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.AddBookToWantToRead(null, null), "Book could not be found");
            Assert.CatchAsync<ArgumentException>(async () => await service.AddBookToWantToRead("abcde", null), "Book could not be found");
        }

        [Test]
        public async Task MarkBookAsReadWorksCorrect()
        {
            var userBooksBefore = await repo.All<UserBook>().ToListAsync();

            var result = service.MarkBookAsRead("abcde", book1Guid.ToString());

            var userBooksAfter = await repo.All<UserBook>().ToListAsync();

            var userBook = repo.All<UserBook>()
                .Where(ub => ub.BookId == book1Guid && ub.ApplicationUserId == "abcde")
                .FirstOrDefault();

            Assert.AreEqual(userBooksBefore.Count(), userBooksAfter.Count() - 1);
            Assert.IsNotNull(userBook);
            Assert.AreEqual(BookConstants.Read, userBook.BookStatus);
        }

        [Test]
        public async Task MarkBookAsReadThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.MarkBookAsRead(null, null), "Book could not be found");
            Assert.CatchAsync<ArgumentException>(async () => await service.MarkBookAsRead("abcde", null), "Book could not be found");
        }

        [Test]
        public async Task DeleteBookWorksCorrect()
        {
            var userBooksBefore = await repo.All<Book>().ToListAsync();

            var result = service.DeleteBook(book1Guid.ToString());

            var userBooksAfter = await repo.All<Book>().ToListAsync();


            Assert.AreEqual(userBooksBefore.Count(), userBooksAfter.Count() + 1);
        }

        [Test]
        public async Task GetAllGenresForBookReturnCorrectData()
        {
            var genres = await service.GetAllGenresForBook(book1Guid.ToString());

            Assert.AreEqual(genres.ToList().Count(), 1);
        }

        [Test]
        public async Task GetAllGenresForBookThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetAllGenresForBook(null), "Movie could not be found");
        }

        [Test]
        public async Task GetAllWantToReadBooksReturnCorrectNumberOfBooks()
        {
            await service.AddBookToWantToRead("abcde", book1Guid.ToString());

            var allWTRBooks = await service.GetAllWantToReadBooks("abcde");

            var wtrBoooks = repo.All<UserBook>()
                .Where(um => um.ApplicationUserId == "abcde" && um.BookStatus == BookConstants.WantToRead)
                .Count();

            Assert.AreEqual(allWTRBooks.ToList().Count(), wtrBoooks);
        }

        [Test]
        public async Task GetAllReadBooksMustReturnCorrectNumberOfBooks()
        {
            await service.MarkBookAsRead("abcde", book1Guid.ToString());

            var allRBooks = await service.GetAllReadBooks("abcde");

            var rBoooks = repo.All<UserBook>()
                .Where(um => um.ApplicationUserId == "abcde" && um.BookStatus == BookConstants.Read)
                .Count();

            Assert.AreEqual(allRBooks.ToList().Count(), rBoooks);
        }

        [Test]
        public async Task GetAllCurrentlyReadingBooksMustReturnCorrectNumberOfBooks()
        {
            await service.AddBookToCurrentlyReading("abcde", book2Guid.ToString());

            var allCRBooks = await service.GetAllCurrentlyReadingBooks("abcde");

            var crBoooks = repo.All<UserBook>()
                .Where(um => um.ApplicationUserId == "abcde" && um.BookStatus == BookConstants.Reading)
                .Count();

            Assert.AreEqual(allCRBooks.ToList().Count(), crBoooks);
        }

        [Test]
        public async Task RemoveBookFromListDecreasesNumberOfUserBooks()
        {
            await service.MarkBookAsRead("abcde", book2Guid.ToString());

            var booksBefore = repo.All<UserBook>()
                .Where(ub => ub.ApplicationUserId == "abcde")
                .Count();
            var result = await service.RemoveBookFromList("abcde", book2Guid.ToString());

            var booksAfter = repo.All<UserBook>()
               .Where(ub => ub.ApplicationUserId == "abcde")
               .Count();

            Assert.AreEqual(booksBefore, booksAfter + 1);
        }

        [TearDown]
        public void TearDown()
        {
            //Do not forget to dispose the dbContext
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var applicationUser = new ApplicationUser()
            {
                Id = "abcde",
                Email = "mytestuser@abv.bg",
                FirstName = "Pesho",
                LastName = "Ivanov",
            };

            await repo.AddAsync(applicationUser);

            var applicationUser2 = new ApplicationUser()
            {
                Id = "efghi",
                Email = "mysecondtestuser@abv.bg",
                FirstName = "Ivan",
                LastName = "Petrov",
            };
            await repo.AddAsync(applicationUser2);

            var genre1 = new BookGenre()
            {
                Id = new System.Guid(),
                Name = "FirstGenre"
            };

            await repo.AddAsync(genre1);

            var genre2 = new BookGenre()
            {
                Id = new System.Guid(),
                Name = "SecondGenre"
            };

            await repo.AddAsync(genre2);

            var book1 = new Book()
            {
                Id = book1Guid,
                Title = "Test Book 1",
                ImageUrl = "https://imdb-api.com/images/original/MV5BMDdmMTBiNTYtMDIzNi00NGVlLWIzMDYtZTk3MTQ3NGQxZGEwXkEyXkFqcGdeQXVyMzMwOTU5MDk@._V1_Ratio0.6837_AL_.jpg",
                Author = "Author 1",
                Isbn13 = "9780062892409",
                Pages = 100,
                Year = 2022,
                Description = "First book description"
            };

            book1.Genres.Add(genre1);
            await repo.AddAsync(book1);

            var book2 = new Book()
            {
                Id = book2Guid,
                Title = "Test Book 2",
                ImageUrl = "https://imdb-api.com/images/original/MV5BMDdmMTBiNTYtMDIzNi00NGVlLWIzMDYtZTk3MTQ3NGQxZGEwXkEyXkFqcGdeQXVyMzMwOTU5MDk@._V1_Ratio0.6837_AL_.jpg",
                Author = "Author 2",
                Isbn13 = "9781542029919",
                Pages = 100,
                Year = 2022,
                Description = "Second book description"
            };

            book2.Genres.Add(genre2);
            await repo.AddAsync(book2);


            await repo.SaveChangesAsync();
        }
    }
}