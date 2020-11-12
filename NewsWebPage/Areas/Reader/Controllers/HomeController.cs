using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsWebPage.Data;
using NewsWebPage.Extensions;
using NewsWebPage.Models;

namespace NewsWebPage.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var postlist = await _db.Posts.Include(m => m.Category).Include(m => m.SpecialTags).ToListAsync();
            return View(postlist);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Posts.Include(m => m.Category).Include(m => m.SpecialTags).Where(m => m.Id == id).FirstOrDefaultAsync();

            return View(product);
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