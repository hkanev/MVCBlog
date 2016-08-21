using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleForum.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Posts = new HashSet<Post>();
        }
        public int Id { get; set; }

        public string Content { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}