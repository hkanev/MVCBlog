using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleForum.Models;

namespace MVCBlog.Models
{
    public class HomeView
    {
        public HomeView()
        {
            this.Tags = new List<Tag>();
            this.Comments = new List<Comment>();
            this.Posts = new List<Post>();
            this.Categories = new List<Category>();
        }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}