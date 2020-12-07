using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebPage.Data;
using NewsWebPage.Extensions;
using NewsWebPage.Models;
using NewsWebPage.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

namespace NewsWebPage.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class PostSaveController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public PostSaveViewModel PostSaveVM { get; set; }

        public PostSaveController(ApplicationDbContext db)
        {
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
            var ReaderID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postList = (IEnumerable<int>)(from ps in _db.PostSaves where ps.ReaderID == ReaderID select ps.PostID).ToList();

            foreach (var postitem in postList)
            {
                Post post = _db.Posts.Include(p => p.SpecialTags).Include(p => p.Category).Where(p => p.Id == postitem).FirstOrDefault();
                PostSaveVM.Posts.Add(post);
            }

            return View(PostSaveVM);
        }

        public IActionResult Remove(int id)
        {
            var ReaderID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var postList = (IEnumerable<int>)(from ps in _db.PostSaves where ps.ReaderID == ReaderID select ps.PostID).ToList();
            if (postList.Count() > 0)
            {
                if (postList.Contains(id))
                {
                    PostSave post = _db.PostSaves.Where(p => p.PostID == id).Where(p => p.ReaderID == ReaderID).FirstOrDefault();
                    _db.PostSaves.Remove(post);
                    _db.SaveChanges();
                }
            }
            _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}