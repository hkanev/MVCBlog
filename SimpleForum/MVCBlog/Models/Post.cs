using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleForum.Models;

namespace SimpleForum.Models
{
    public class Post
    {
        public Post()
        {
            this.Date = DateTime.Now;
            this.Tags= new HashSet<Tag>();
            this.Comment= new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [AllowHtml]
        [UIHint("tinymce_full")]
        public string Body { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ForeignKey("Author")]
        public string Author_Id { get; set; }

        public ApplicationUser Author { get; set; }

        [ForeignKey("Category")]
        public int Category_Id { get; set; }

        public Category Category { get; set; }

        public byte[] PostPicture { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }

    }
}