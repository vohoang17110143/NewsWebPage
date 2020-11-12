using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebPage.Data;
using NewsWebPage.Models;
using NewsWebPage.Models.ViewModel;
using NewsWebPage.Utility;

namespace NewsWebPage.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public PostViewModel PostVM { get; set; }

        public PostController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            PostVM = new PostViewModel()
            {
                Post = new Models.Post(),

                Categories = _context.Categories,
                SpecialTags = _context.SpecialTags
            };
        }

        public async Task<IActionResult> Index()
        {
            var post = _context.Posts.Include(m => m.SpecialTags).Include(m => m.Category).Include(m => m.SpecialTags);
            return View(await post.ToListAsync());
        }

        public IActionResult Create()
        {
            return View(PostVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(PostVM);
            }

            _context.Posts.Add(PostVM.Post);

            await _context.SaveChangesAsync();

            //lưu ảnh
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var postFromDb = _context.Posts.Find(PostVM.Post.Id);
            postFromDb.DateCreated = DateTime.Now;

            if (files.Count != 0)
            {
                //Image will be upload
                var uploads = Path.Combine(webRootPath, SD.DefaultImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, PostVM.Post.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                postFromDb.Image = @"\" + SD.DefaultImageFolder + @"\" + PostVM.Post.Id + extension;
            }
            else
            {
                //when not upload image
                var uploads = Path.Combine(webRootPath, SD.DefaultImageFolder + @"\" + SD.DefaultImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.DefaultImageFolder + @"\" + PostVM.Post.Id + ".png");
                postFromDb.Image = @"\" + SD.DefaultImageFolder + @"\" + PostVM.Post.Id + ".png";
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostVM.Post = await _context.Posts.Include(m => m.SpecialTags).Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (PostVM.Post == null)
            {
                return NotFound();
            }

            return View(PostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = _context.Posts.Where(m => m.Id == PostVM.Post.Id).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    var uploads = Path.Combine(webRootPath, SD.ProductImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, PostVM.Post.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, PostVM.Post.Id + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, PostVM.Post.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    PostVM.Post.Image = @"\" + SD.ProductImageFolder + @"\" + PostVM.Post.Id + extension_new;
                }

                if (PostVM.Post.Image != null)
                {
                    productFromDb.Image = PostVM.Post.Image;
                }

                productFromDb.Name = PostVM.Post.Name;

                productFromDb.Author = PostVM.Post.Author;
                productFromDb.DateCreated = DateTime.Now;
                productFromDb.Content = PostVM.Post.Content;
                productFromDb.Description = PostVM.Post.Description;
                productFromDb.CategoryID = PostVM.Post.CategoryID;

                productFromDb.SpecialTagsID = PostVM.Post.SpecialTagsID;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(PostVM);
        }

        //GET : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostVM.Post = await _context.Posts.Include(m => m.Category).Include(m => m.SpecialTags).SingleOrDefaultAsync(m => m.Id == id);

            if (PostVM.Post == null)
            {
                return NotFound();
            }

            return View(PostVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PostVM.Post = await _context.Posts.Include(m => m.Category).Include(m => m.SpecialTags).SingleOrDefaultAsync(m => m.Id == id);

            if (PostVM.Post == null)
            {
                return NotFound();
            }

            return View(PostVM);
        }

        //POST : Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Post post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ProductImageFolder);
                var extension = Path.GetExtension(post.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, post.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, post.Id + extension));
                }
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
    }
}