using System.Collections.Generic;
using MVCBlog.Models.DataModels;

namespace MVCBlog.Models.ViewModels
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