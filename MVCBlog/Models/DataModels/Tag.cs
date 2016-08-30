using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Models.DataModels
{
    public class Tag 
    {
        public Tag()
        {
            
        }
        public Tag(string name)
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
