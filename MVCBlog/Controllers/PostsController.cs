﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCBlog.Extensions;
using MVCBlog.Models;
using MVCBlog.Models.DataModels;
using MVCBlog.Models.ViewModels;
using PagedList;

namespace MVCBlog.Controllers
{
    [ValidateInput(false)]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            var post = db.Posts.Include(p => p.Author).Include(p => p.Category).ToList();
            return View(post);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }
            var postViewModel = new PostViewModel();
            var post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            postViewModel.Post = post;
            var comments =
                db.Comments.Include(c => c.Post).Where(c => c.PostId == postViewModel.Post.Id).OrderByDescending(c => c.Date).ToList();
            postViewModel.Comments = comments;
            var commentsAside =
                db.Comments.ToList().OrderByDescending(t => t.Date).Take(5).ToList();
            postViewModel.CommentAside = commentsAside;
            var categories = db.Categories.ToList();
            postViewModel.Categories = categories;
            var tags = db.Tags.OrderByDescending(t => t.Posts.Count).Take(9).ToList();
            postViewModel.Tags = tags;

            if (ModelState.IsValid)
            {
                post.Views = post.Views + 1;
                db.SaveChanges();
            }

            if (postViewModel.Post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }

            
            var pageNumber = page ?? 1;
            var onePageOfComments = comments.ToPagedList(pageNumber, 5);
            ViewBag.onePageOfComments = onePageOfComments;

            return View(postViewModel);
        }

        public FileContentResult Photo(int? id)
        {
            Post post = db.Posts.Find(id);

            return new FileContentResult(post.PostPicture, "image/jpeg");
            
        }

        [Authorize]
        [HttpGet]
        public ActionResult SetPicture(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == post.Author.UserName)
            {
                return View();
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPicture(HttpPostedFileBase Profile, int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }
            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == post.Author.UserName)
            {
                byte[] image = new byte[Profile.ContentLength];
                Profile.InputStream.Read(image, 0, Convert.ToInt32(Profile.ContentLength));
                post.PostPicture = image;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
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
                post.Views = 0;
                db.Posts.Add(post);
                db.SaveChanges();
                this.AddNotification("Post created", NotificationType.INFO);
                return RedirectToAction("Details", "Posts", new { id = post.Id });
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
                return RedirectToAction("Index","Home");
            }

            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == post.Author.UserName)
            {
                ViewBag.Categories = db.Categories.ToList();
                ViewBag.Tags = new MultiSelectList(db.Tags, "Id", "Name");
                return View(post);
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Description,Category_Id,")] Post post, FormCollection form)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = new MultiSelectList(db.Tags, "Id", "Name");

                string selectedValues = form["Tags"];
                int[] id = null;
                if (selectedValues != null)
                {
                    var tags = selectedValues.Split(',');
                    id = tags.Select(int.Parse).ToArray();
                }
                if (ModelState.IsValid)
                {
                    var item = db.Entry(post);
                    item.State = EntityState.Modified;
                    item.Collection(x => x.Tags).Load();
                    if (selectedValues != null)
                    {
                        post.Tags.Clear();
                        foreach (var tagid in id)
                        {
                            var tag = db.Tags.FirstOrDefault(t => t.Id == tagid);
                            post.Tags.Add(tag);
                        }
                    }
                    post.Author_Id = User.Identity.GetUserId();
                    db.SaveChanges();
                    this.AddNotification("Post edited.", NotificationType.INFO);
                    return RedirectToAction("Index","Home");
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
                return RedirectToAction("Index", "Home");
            }
            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id ==id);
            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == post.Author.UserName)
            {
                return View(post);
            }
           
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (User.IsInRole("Administrators") || User.Identity.Name == post.Author.UserName)
            {
                post.Comment.Clear();
                post.Tags.Clear();
                db.Posts.Remove(post);
                db.SaveChanges();
                this.AddNotification("Post deleted.", NotificationType.INFO);
                return RedirectToAction("Index","Home");
            }
            this.AddNotification("You are not authorized..", NotificationType.ERROR);
            return RedirectToAction("Index","Home");
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

