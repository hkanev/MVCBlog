using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCBlog.Extensions;
using MVCBlog.Models;
using MVCBlog.Models.DataModels;

namespace MVCBlog.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Administrators")]
        // GET: Comments
        public ActionResult Index()
        {   
            var comments = db.Comments.Include(c => c.Post);
            return View(comments.ToList());
        }

        [Authorize(Roles = "Administrators")]
        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            var authors = db.Posts.Include(p => p.Author);
            ViewBag.Authors = authors;
            if (id == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        [Authorize]
        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }
    
    
        
        [Authorize]
        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,PostId,Date")] Comment comment, int? id)
        {
            if (id == null)
            {
                this.AddNotification("Post can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                this.AddNotification("Post can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                comment.Post = post;
                comment.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("Comment created", NotificationType.INFO);
                return RedirectToAction("Details", "Posts", new {id = post.Id});
            }
           
            return View(comment);
        }

        [Authorize]
        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {   
            if (id == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            var comment = db.Comments.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index","Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == comment.Author.UserName)
            {
                return View(comment);
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content")] Comment comment,int? id, int? postId)
        {
            Post post = db.Posts.Find(postId);
            if (User.IsInRole("Administrators") || User.Identity.Name == comment.Author.UserName)
            {
                if (ModelState.IsValid)
                {
                    comment.PostId = post.Id;
                    comment.Author_Id = User.Identity.GetUserId();
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                    this.AddNotification("Comment edited", NotificationType.INFO);
                    return RedirectToAction("Index","Home");
                }
                return View(comment);
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            var comment = db.Comments.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                this.AddNotification("Comment can not be found.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole("Administrators") || User.Identity.Name == comment.Author.UserName)
            {
                return View(comment);
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var comment = db.Comments.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            if (User.IsInRole("Administrators") || User.Identity.Name == comment.Author.UserName)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                this.AddNotification("Comment deleted", NotificationType.INFO);
                return RedirectToAction("Index", "Home");
            }
            this.AddNotification("You are not authorized.", NotificationType.ERROR);
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
