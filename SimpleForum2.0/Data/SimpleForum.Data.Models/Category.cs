using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}