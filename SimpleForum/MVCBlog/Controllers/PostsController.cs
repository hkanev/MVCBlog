using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.DynamicData.ModelProviders;
using System.Web.Mvc;
using SimpleForum.Models;
using SimpleForum.Extensions;
using Microsoft.AspNet.Identity;
using MVCBlog.Models;
using PagedList;

namespace SimpleForum.Controllers
{
    [ValidateInput(false)]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var post = db.Posts.Include(p => p.Author).ToList();
            return View(post);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
            var postViewModel = new PostViewModel();
            postViewModel.Post = db.Posts.Find(id);
            var comments =
                db.Comments.Include(c => c.Post).Where(c => c.PostId == postViewModel.Post.Id).ToList();
            postViewModel.Comments = comments;
            var categories = db.Categories.ToList();

            if (postViewModel.Post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }

            
            var pageNumber = page ?? 1;
            var onePageOfComments = comments.ToPagedList(pageNumber, 10);
            ViewBag.onePageOfComments = onePageOfComments;

            return View(postViewModel);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = new MultiSelectList(db.Tags,"Id","Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Title,Body,Date,Description,Category_Id")] Post post, FormCollection form)
        {
            ViewBag.Categories = db.Categories.ToList();
            string selectedValues = form["Tags"];
            var tags = selectedValues.Split(',');
            int[] id = tags.Select(int.Parse).ToArray();
            if (ModelState.IsValid)
            {
                if (post == null)
                {
                    return HttpNotFound();
                }
                foreach (var tagid in id)
                {
                    var tag = db.Tags.FirstOrDefault(t => t.Id == tagid);
                    post.Tags.Add(tag);
                }
                post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                post.Author_Id = User.Identity.GetUserId();
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

