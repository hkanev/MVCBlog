using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForum.Data;
using SimpleForum.Data.Contracts.Repository;
using SimpleForum.Data.Models;
using AutoMapper.QueryableExtensions;
using SimpleForum.Web.ViewModels;

namespace SimpleForum.Web.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Post> posts;
        private IRepository<Comment> comments;
      

        public HomeController(IRepository<Post> posts, IRepository<Comment> comments)
        {
            this.posts = posts;
            this.comments = comments;
        }
        public ActionResult Index()
        {
            var post = posts.All().Project().To<IndexForumPostViewModel>();
            return View(post);
        }

    }
}