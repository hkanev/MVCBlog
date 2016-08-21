using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleForum.Data.Contracts.Models;
using SimpleForum.Data.Migrations;
using SimpleForum.Data.Models;

namespace SimpleForum.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public IDbSet<Post> Posts { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }

    }
}