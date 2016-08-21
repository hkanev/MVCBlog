using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SimpleForum.Models;

namespace SimpleForum.Models
{
    public class Comment
    {
        public Comment()
        {
            
        }
        public Comment(int postId, string content, ApplicationUser author = null)
        {
            this.PostId = postId;
            this.Content = content;
            this.Author = author;
            this.Posts = new HashSet<Post>();
        }
        public int Id { get; set; }

        public string Content { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}