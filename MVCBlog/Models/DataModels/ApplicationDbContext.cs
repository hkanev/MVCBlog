using System.Collections;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVCBlog.Models.DataModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }

    }
}