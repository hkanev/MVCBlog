using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimpleForum.Data.Contracts.Models;

namespace SimpleForum.Data.Models
{
    public class Tag 
    {
        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }
        
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }
}
