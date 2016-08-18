using System.Collections.Generic;

namespace SimpleForum.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using MVC_Blog.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {

            var tagList = new List<Tag>();
            if (!context.Tags.Any())
            {
                for (int i = 1; i < 21; i++)
                {
                    var tag = new Tag() {Name = $"tag {i}"};
                    context.Tags.Add(tag);
                    tagList.Add(tag);
                }
            }
            if (!context.Posts.Any())
            {
                var tagIndex = 0;
                for (int i = 1; i < 21; i++)
                {
                    var post = new Post() {Body = $"Body content {i}", Title = $"Post title {i}"};
                    for (int j = 0; j < 5; j++)
                    {
                        post.Tags.Add(tagList[tagIndex%tagList.Count]);
                        tagIndex++;
                    }
                    context.Posts.Add(post);
                }
            }
            context.SaveChanges();

        }
    }
}
