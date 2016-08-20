using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Models;
using MVC_Blog.Models;
using SimpleForum.Extensions;
using Microsoft.AspNet.Identity;

namespace SimpleForum.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.Include(p => p.Author).ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Title,Body,Date")] Post post, int? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category category = db.Categories.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                post.Category = category;
                post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Posts.Add(post);
                db.SaveChanges();
                this.AddNotification("Post created", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }

            var authors = db.Users.ToList();
            ViewBag.Authors = authors;
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,Author_Id")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Post edited.", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [Authorize]
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            this.AddNotification("Post deleted.", NotificationType.INFO);
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

