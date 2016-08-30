using System.Collections.Generic;
using MVCBlog.Models.DataModels;

namespace MVCBlog.Models.ViewModels
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