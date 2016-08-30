using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using MVCBlog.Models;
using SimpleForum.Extensions;
using SimpleForum.Models;
using PagedList;

namespace SimpleForum.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            var category = db.Categories.ToList();
            return View(category);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? page,int? id)
        {
            if (id == null)
            {
                this.AddNotification("Category cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }

            var blogViewModels = new BlogViewModels();
            blogViewModels.Comments = db.Comments.Include((c => c.Author)).OrderByDescending(c => c.Date).Take(5).ToList();
            blogViewModels.Categories = db.Categories.ToList();
            blogViewModels.Tags = db.Tags.OrderByDescending(t => t.Posts.Count).Take(9).ToList();
            blogViewModels.Category = db.Categories.Find(id);
            var posts = db.Posts.Where(p => p.Category.Id == blogViewModels.Category.Id).ToList();
            blogViewModels.Posts = posts.OrderByDescending(p => p.Date).ToList();

            var pageNumber = page ?? 1;
            var onePageOfPosts = posts.ToPagedList(pageNumber, 2);
            ViewBag.onePageOfPosts = onePageOfPosts;

            if (blogViewModels.Category == null)
            {
                this.AddNotification("Category cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }

            return View(blogViewModels);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                this.AddNotification("Category created", NotificationType.INFO);
                return RedirectToAction("Index", "Home");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Category can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                this.AddNotification("Category can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Category edited.", NotificationType.INFO);
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Category can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                this.AddNotification("Category can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            this.AddNotification("Category deleted", NotificationType.INFO);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
