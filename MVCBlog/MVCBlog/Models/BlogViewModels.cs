using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForum.Models;
using MvcPaging;
using System.Collections;

namespace MVCBlog.Models
{
    public class BlogViewModels
    {
        public BlogViewModels()
        {
            this.Tags = new List<Tag>();
            this.Comments = new List<Comment>();
            this.Posts = new List<Post>();
            this.Categories = new List<Category>();
        }

        public Category Category { get; set; }
        public Tag Tag { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    
}
}