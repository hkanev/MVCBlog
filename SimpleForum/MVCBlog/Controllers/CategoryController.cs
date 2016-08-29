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
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var category = from s in db.Categories
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    category = category.Where(c => c.Id==1);
                    break;
                default:
                    category = category.OrderBy(s => s.Name);
                    break;
            }
            return View(category.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? page,int? id)
        {
            if (id == null)
            {
                this.AddNotification("Category cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }

            return View(blogViewModels);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
