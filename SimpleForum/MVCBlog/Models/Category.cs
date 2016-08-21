using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleForum.Models;

namespace SimpleForum.Models
{
    public class Category
    {
        public Category()
        {
            
        }
        public Category(string name)
        {
            this.Name = name;
            this.Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}