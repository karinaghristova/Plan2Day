using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Models;

namespace Plan2Day.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService bookService;

        private readonly UserManager<ApplicationUser> userManager;

        public BookController(IBookService bookService,
            UserManager<ApplicationUser> userManager)
        {
            this.bookService = bookService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllBooks(int page = 1)
        {
            var books = await bookService.GetAllBooks();
            int booksPerPage = PageConstants.PageSize20;
            int booksToSkip = page == 1 ? 0 : page * booksPerPage;

            return View(new AllBooksViewModel
            {
                Books = books.Skip(booksToSkip).Take(booksPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(books.Count() / (double)booksPerPage)
                }
            });
        }

        public async Task<IActionResult> WantToReadBooks(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var books = await bookService.GetAllWantToReadBooks(userId);
            int booksPerPage = PageConstants.PageSize20;
            int booksToSkip = page == 1 ? 0 : page * booksPerPage;

            return View(new AllBooksViewModel
            {
                Books = books.Skip(booksToSkip).Take(booksPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(books.Count() / (double)booksPerPage)
                }
            });
        }

        public async Task<IActionResult> CurrentlyReadingBooks(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var books = await bookService.GetAllCurrentlyReadingBooks(userId);
            int booksPerPage = PageConstants.PageSize20;
            int booksToSkip = page == 1 ? 0 : page * booksPerPage;

            return View(new AllBooksViewModel
            {
                Books = books.Skip(booksToSkip).Take(booksPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(books.Count() / (double)booksPerPage)
                }
            });
        }

        public async Task<IActionResult> ReadBooks(int page = 1)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var books = await bookService.GetAllReadBooks(userId);
            int booksPerPage = PageConstants.PageSize20;
            int booksToSkip = page == 1 ? 0 : page * booksPerPage;

            return View(new AllBooksViewModel
            {
                Books = books.Skip(booksToSkip).Take(booksPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(books.Count() / (double)booksPerPage)
                }
            });
        }

        public async Task<IActionResult> AddBookToWantToRead(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.AddBookToWantToRead(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully added book to want to read list.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Book could not be added to want to read list.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Book could not be added to want to read list.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("WantToReadBooks");
        }

        public async Task<IActionResult> AddBookRead(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.MarkBookAsRead(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully added book to read list.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Book could not be added to read list.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Book could not be added to read list.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ReadBooks");
        }

        public async Task<IActionResult> AddBookToCurrentlyReading(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.AddBookToCurrentlyReading(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully added book to currently reading list.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Book could not be added to currently reading list.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Book could not be added to to currently reading list.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("CurrentlyReadingBooks");
        }

        public async Task<IActionResult> RemoveBookFromWantToReadList(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.RemoveBookFromList(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully removed movie";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("WantToReadBooks");

        }

        public async Task<IActionResult> RemoveBookFromReadList(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.RemoveBookFromList(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully removed movie";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ReadBooks");

        }

        public async Task<IActionResult> RemoveBookFromCurrentlyReadingList(string bookId)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            try
            {
                var result = await bookService.RemoveBookFromList(userId, bookId);

                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully removed movie";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Movie could not be removed.";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("CurrentlyReadingBooks");

        }

        public async Task<IActionResult> BookDetails(string bookId)
        {
            try
            {
                var resultModel = await bookService.GetBookDetails(bookId);
                ViewData[MessageConstant.SuccessMessage] = "Book details successfully loaded.";
                return View(resultModel);

            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Book details could not be loaded.";
                return RedirectToAction("AllBooks");
            }

        }
    }
}
