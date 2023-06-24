using BookShoppingCart.Data;
using BookShoppingCart.Models;
using BookShoppingCart.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _context;

        public HomeController(ILogger<HomeController> logger,IHomeRepository applicationDbContext)
        {
            _logger = logger;
            _context = applicationDbContext;
        }

        public async Task<IActionResult> Index(string? sTerm = "", int GenreId = 0)
        {
           
            IEnumerable<Book> books =await _context.GetBooks(sTerm,GenreId);
            IEnumerable<Genre> genres =await _context.GenreList();
            BookDisplayModel BookModel = new BookDisplayModel
            {
                Books = books,
                Genres = genres,
                sTerm= sTerm,
                GenreId=GenreId
            };

            return View(BookModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}