using Microsoft.AspNetCore.Mvc;
using Plan2Day.Core.Constants;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Models.Books;
using Plan2Day.Models;

namespace Plan2Day.Areas.Admin.Controllers
{
    public class BookController : BaseController
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageBooks(int page = 1)
        {
            var movies = await bookService.GetAllBooks();
            int moviesPerPage = PageConstants.PageSize20;
            int moviesToSkip = page == 1 ? 0 : page * moviesPerPage;

            return View(new AllBooksViewModel
            {
                Books = movies.Skip(moviesToSkip).Take(moviesPerPage),
                Paging = new PagingViewModel
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(movies.Count() / (double)moviesPerPage)
                }
            });
        }

        public async Task<IActionResult> DeleteBook(string bookId)
        {
            try
            {
                var result = await bookService.DeleteBook(bookId);
                if (result == true)
                {
                    ViewData[MessageConstant.SuccessMessage] = "Successfully deleted book.";
                }
                else
                {
                    ViewData[MessageConstant.ErrorMessage] = "Book could not be deleted";
                }
                return RedirectToAction("ManageBooks");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> EditBook(string bookId)
        {
            try
            {
                var model = await bookService.EditBook(bookId);

                return View(model);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Book could not be edited.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await bookService.UpdateBook(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Successfully edited book.";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Book could not be edited.";
            }

            return View(model);
        }
    }
}
