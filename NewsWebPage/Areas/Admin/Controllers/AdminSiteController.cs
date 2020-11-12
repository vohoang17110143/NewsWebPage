using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebPage.Data;
using NewsWebPage.Models.ViewModel;
using NewsWebPage.Utility;

namespace NewsWebPage.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class AdminSiteController : Controller
    {
        private readonly ApplicationDbContext _db;

        public IActionResult Index()
        {
            StatisticalViewModel board = new StatisticalViewModel();
            //board.Posts_Count = _db.Posts.Count();
            //board.Authors_Count = _db.Authors.Count();
            return View();
        }
    }
}