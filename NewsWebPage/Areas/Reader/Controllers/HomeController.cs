using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsWebPage.Data;
using NewsWebPage.Extensions;
using NewsWebPage.Models;
using NewsWebPage.Models.ViewModel;
using RestSharp;

namespace NewsWebPage.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostSaveViewModel PostSaveVM { get; set; }

        public int PiD;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _db = db;

            PostSaveVM = new PostSaveViewModel()
            {
                Posts = new List<Post>(),
                Reader = new ApplicationUser(),
                PostSave = new PostSave()
            };
        }

        public async Task<IActionResult> Index()
        {
            var postlist = await _db.Posts.Include(m => m.Category).Include(m => m.SpecialTags).ToListAsync();
            return View(postlist);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _db.Posts.Include(m => m.Category)
                                        .Include(m => m.SpecialTags)
                                        .Where(m => m.Id == id).FirstOrDefaultAsync();
            var postViewCount = await _db.Posts.FindAsync(id);
            postViewCount.ViewCount += 1;
            _db.Entry(post);

            await _db.SaveChangesAsync();

            return View(post);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            //List<int> PostSavePage = HttpContext.Session.Get<List<int>>("PostSaveBar");
            //if (PostSavePage == null)
            //{
            //    PostSavePage = new List<int>();
            //}
            //PostSavePage.Add(id);
            // HttpContext.Session.Set("PostSaveBar", PostSavePage);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                PostSaveVM.PostSave.ReaderID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                PostSaveVM.PostSave.PostID = id;
            }
            PostSaveVM.PostSave.DateRead = DateTime.Now;

            PostSave postSave = PostSaveVM.PostSave;
            _db.PostSaves.Add(postSave);
            _db.SaveChanges();

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { area = "Reader" });
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