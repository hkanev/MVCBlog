using SimpleForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using MVCBlog.Models;

namespace SimpleForum.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var homeView = new HomeView();
            homeView.Posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(5).ToList();
            homeView.Comments = db.Comments.Include((c => c.Author)).OrderByDescending(c => c.Date).Take(5).ToList();
            homeView.Categories = db.Categories.ToList();
       
            homeView.Tags = db.Tags.ToList();

            return View(homeView);
        }
    }
}