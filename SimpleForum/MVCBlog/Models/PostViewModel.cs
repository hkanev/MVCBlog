using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleForum.Models;

namespace MVCBlog.Models
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            this.Tags = new List<Tag>();
            this.Comments = new List<Comment>();
            this.CommentAside = new List<Comment>();
            this.Categories = new List<Category>();
        }

        public Post Post { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Comment> CommentAside { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}