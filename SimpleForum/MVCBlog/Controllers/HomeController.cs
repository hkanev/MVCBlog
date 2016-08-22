using SimpleForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using MVCBlog.Models;
using PagedList;

namespace SimpleForum.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            var blogViewModels = new BlogViewModels();
            blogViewModels.Posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(5).ToList();
            blogViewModels.Comments = db.Comments.Include((c => c.Author)).OrderByDescending(c => c.Date).Take(5).ToList();
            blogViewModels.Categories = db.Categories.ToList();
            blogViewModels.Tags = db.Tags.ToList();

            var postList = db.Posts.ToList();
            var pageNumber = page ?? 1;
            var onePageOfPosts = postList.ToPagedList(pageNumber, 5); 
            ViewBag.onePageOfPosts = onePageOfPosts;

            return View(blogViewModels);
        }
    }
}