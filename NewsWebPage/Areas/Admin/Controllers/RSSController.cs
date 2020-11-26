using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebPage.Common;
using NewsWebPage.Data;
using NewsWebPage.Models;
using NewsWebPage.Models.ViewModel;
using NewsWebPage.Utility;

namespace NewsWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RSSController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RSSFeed RSSVM;

        public string data;

        public RSSController(ApplicationDbContext db)
        {
            _db = db;
            RSSVM = new RSSFeed();
        }

        public ActionResult PostFeed(string type)
        {
            Category category = _db.Categories.Where(s => s.Alias.Contains(type)).FirstOrDefault();
            if (category == null)
            {
                return BadRequest();
            }
            IEnumerable<Article> posts = (from s in _db.Articles where s.Category.Alias.Contains(type) select s).ToList();
            var feed = new SyndicationFeed(category.Name, "RSS Feed",
                new Uri("https://myblog.com/RSS"),
                Guid.NewGuid().ToString(),
                DateTime.Now
                );
            var items = new List<SyndicationItem>();
            foreach (Article art in posts)
            {
                string postUrl = String.Format("https://myblog.com/" + art.Alias + "-{0}", art.Id);
                var item =
                new SyndicationItem(Helper.RemoveIllegalCharacters(art.Title),
                Helper.RemoveIllegalCharacters(art.Description),
                new Uri(postUrl),
                art.Id.ToString(),
                art.DatePublished.Value
                );

                items.Add(item);
            }

            feed.Items = items;
            return new RSSActionResult { Feed = feed };
        }

        public ActionResult ReadRSS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadRSS(string url)
        {
            WebClient wclient = new WebClient();
            wclient.Encoding = ASCIIEncoding.UTF8;
            if (url == null)
            {
                return BadRequest();
            };
            string RSSData = wclient.DownloadString(url);
            XDocument xml = XDocument.Parse(RSSData, LoadOptions.PreserveWhitespace);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RSSFeed
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   Description = ((string)x.Element("description")),
                                   PubDate = ((string)x.Element("pubDate"))
                               }
                               );

            ViewBag.RSSFeed = RSSFeedData;
            ViewBag.URL = url;

            return View();
        }

        [HttpPost, ActionName("SaveRSS")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRSS(string url)
        {
            WebClient wclient = new WebClient();
            wclient.Encoding = ASCIIEncoding.UTF8;
            string RSSData = wclient.DownloadString(url);
            XDocument xml = XDocument.Parse(RSSData, LoadOptions.PreserveWhitespace);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new Article
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   Description = ((string)x.Element("description")),
                                   DatePublished = ((DateTime)x.Element("pubDate"))
                               }
                               );

            if (!ModelState.IsValid)
            {
                return View(RSSVM);
            }

            _db.Articles.Add(RSSFeedData.FirstOrDefault());
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(ReadRSS));
        }
    }
}