using MVC_Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SimpleForum.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(3);

            return View(posts.ToList());
        }
    }
}