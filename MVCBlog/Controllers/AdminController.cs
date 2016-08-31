using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Microsoft.Ajax.Utilities;
using MVCBlog.Models;
using PagedList;
using Microsoft.AspNet.Identity.Owin;
using MVCBlog.Extensions;
using MVCBlog.Models.DataModels;
using MVCBlog.Models.ViewModels;

namespace MVCBlog.Controllers
{
    [ValidateInput(false)]
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            var blogViewModels = new BlogViewModels();
            blogViewModels.Comments = db.Comments.ToList();
            blogViewModels.Categories = db.Categories.ToList();
            blogViewModels.Tags = db.Tags.ToList();
            blogViewModels.Posts = db.Posts.Include(p => p.Author).ToList();

            return View(blogViewModels);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }

            Post post = db.Posts.Find(id);

            if (post == null)
            {
                this.AddNotification("Post cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = new MultiSelectList(db.Tags, "Id", "Name");
            ViewBag.Authors = db.Users.ToList();
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPost([Bind(Include = "Id,Title,Body,Date,Description,Category_Id,Author_Id")] Post post, FormCollection form)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = new MultiSelectList(db.Tags, "Id", "Name");
            ViewBag.Authors = db.Users.ToList();
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
                db.SaveChanges();
                this.AddNotification("Post edited.", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Comments/Edit/5
        public ActionResult EditComment(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Comment cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                this.AddNotification("Comment cant be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            ViewBag.Authors = db.Users.ToList();
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "Id,Content,PostId,Author_Id")] Comment comment)
        {
            if (User.IsInRole("Administrators") || User.Identity.Name == comment.Author.UserName)
            {
                if (ModelState.IsValid)
                {

                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification("Comment edited", NotificationType.INFO);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
                ViewBag.Authors = db.Users.ToList();
                return View(comment);
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

    }
}