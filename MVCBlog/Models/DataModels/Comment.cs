﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBlog.Models.DataModels
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;

        }
        public Comment(int postId, string content, ApplicationUser author = null)
        {
            this.PostId = postId;
            this.Content = content;
            this.Author = author;
            this.Posts = new HashSet<Post>();
            this.Date = DateTime.Now;

        }
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Author")]
        public virtual string Author_Id { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}