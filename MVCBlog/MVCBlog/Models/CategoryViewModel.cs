using SimpleForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBlog.Models
{
    public class CategoryViewModel
    { 
        public CategoryViewModel()
        {
            this.Tags = new List<Tag>();
            this.Comments = new List<Comment>();
            this.Posts = new List<Post>();
        }

        public Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}