using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleForum.Models;

namespace MVCBlog.Models
{
    public class PostVote
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public VoteType type { get; set; }
    }

  
}