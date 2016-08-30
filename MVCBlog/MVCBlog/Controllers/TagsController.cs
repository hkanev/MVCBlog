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

namespace MVCBlog.Controllers
{
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Administrators")]
        // GET: Tags
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? page, int? id)
        {
            if (id == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }

            var blogViewModels = new BlogViewModels();
            blogViewModels.Comments = db.Comments.Include((c => c.Author)).OrderByDescending(c => c.Date).Take(5).ToList();
            blogViewModels.Categories = db.Categories.ToList();
            blogViewModels.Tags = db.Tags.OrderByDescending(t => t.Posts.Count).Take(9).ToList();
            blogViewModels.Tag = db.Tags.Find(id);
            var posts = db.Posts.Where(p => p.Tags.Select(t => t.Id).Contains((int)id)).ToList();
            blogViewModels.Posts = posts.OrderByDescending(p => p.Date).ToList();

            var pageNumber = page ?? 1;
            var onePageOfPosts = posts.ToPagedList(pageNumber, 2);
            ViewBag.onePageOfPosts = onePageOfPosts;

            if (blogViewModels.Tag == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }

            return View(blogViewModels);
        }

        // GET: Tags/Create
        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                this.AddNotification("Tag created", NotificationType.INFO);
                return RedirectToAction("Index","Home");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Tag edited.", NotificationType.INFO);
                return RedirectToAction("Index","Home");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                this.AddNotification("Tag cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            db.SaveChanges();
            this.AddNotification("Tag deleted.", NotificationType.INFO);
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
