using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewsWebPage.Areas.Admin.Controllers
{
    public class RSSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
