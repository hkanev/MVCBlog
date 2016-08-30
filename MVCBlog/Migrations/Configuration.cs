using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVCBlog.Models;
using MVCBlog.Models.DataModels;
using System.Web;
namespace MVCBlog.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {

            if (!context.Users.Any())
            {
                // If the database is empty, populate sample data in it

                var avatarPath = HttpContext.Current.Server.MapPath("~/Content/Images/Users/avatar.jpg");
                Image avatar = Image.FromFile(avatarPath);
                byte[] avatarPic = imgToByteArray(avatar);

                var avatarPath1 = HttpContext.Current.Server.MapPath("~/Content/Images/Users/avatar1.jpg");
                Image avatar1 = Image.FromFile(avatarPath1);
                byte[] avatar1Pic = imgToByteArray(avatar1);

                var avatarPath2 = HttpContext.Current.Server.MapPath("~/Content/Images/Users/avatar2.jpg");
                Image avatar2 = Image.FromFile(avatarPath2);
                byte[] avatar2Pic = imgToByteArray(avatar2);

                var avatarPath3 = HttpContext.Current.Server.MapPath("~/Content/Images/Users/avatar3.jpg");
                Image avatar3 = Image.FromFile(avatarPath3);
                byte[] avatar3Pic = imgToByteArray(avatar3);

                CreateUser(context, "admin@gmail.com", "123", "System Administrator", avatarPic);
                CreateUser(context, "pesho@gmail.com", "123", "Peter Ivanov", avatar2Pic);
                CreateUser(context, "merry@gmail.com", "123", "Maria Petrova", avatar3Pic);
                CreateUser(context, "geshu@gmail.com", "123", "George Petrov", avatar1Pic);

                CreateRole(context, "Administrators");
                AddUserToRole(context, "admin@gmail.com", "Administrators");


                var Vuelta = new Tag { Name = "Vuelta a Espana" };
                context.Tags.Add(Vuelta);
                var Giro = new Tag { Name = "Giro d'Italia" };
                context.Tags.Add(Giro);
                var Tour = new Tag { Name = "Tour de france" };
                context.Tags.Add(Tour);
                var Classics = new Tag { Name = "Classics" };
                context.Tags.Add(Classics);
                var Olympic = new Tag { Name = "OlympicGames" };
                context.Tags.Add(Olympic);
                var WorldChampionship = new Tag { Name = "WorldChampionship" };
                context.Tags.Add(WorldChampionship);
                var Recovery = new Tag { Name = "Recovery" };
                context.Tags.Add(Recovery);
                var Training = new Tag { Name = "Training" };
                context.Tags.Add(Training);
                var Route = new Tag { Name = "Route" };
                context.Tags.Add(Route);
                var UCI = new Tag { Name = "UCI" };
                context.Tags.Add(UCI);
                var ASO = new Tag { Name = "A.S.O" };
                context.Tags.Add(ASO);
                var Guide = new Tag { Name = "Guide" };
                context.Tags.Add(Guide);
                var Inovasion = new Tag { Name = "Inovasion" };
                context.Tags.Add(Inovasion);
                var Мaintenance = new Tag { Name = "Мaintenance" };
                context.Tags.Add(Мaintenance);
                var News = new Tag { Name = "News" };
                context.Tags.Add(News);


                var Road = new Category("Road");
                context.Categories.Add(Road);
                var Mountain = new Category("Mountain");
                context.Categories.Add(Mountain);
                var Tech = new Category("Tech");
                context.Categories.Add(Tech);
                var Track = new Category("Track");
                context.Categories.Add(Track);

                var tourPath= HttpContext.Current.Server.MapPath("~/Content/Images/Posts/Tour2016.jpg");
                Image tour = Image.FromFile(tourPath);
                byte[] tourPic = imgToByteArray(tour);
                CreatePost(context,
                    title: "The Winners of the 2016 Tour de France",
                    body: @"Chris Froome sealed a third career Tour de France victory and a fourth for Team Sky as the Briton safely negotiated the final stage into Paris. The Briton enjoyed a relaxed start to the 21st stage, sharing beers then champagne with his teammates before the peloton arrived on the Champs-Élysées and the sprint teams took over.
                            For the second year running, it was Andre Greipel taking the stage win as the German fastman made it stages for Lotto-Soudal. Mechanicals saw Marcel Kittel and Bryan Coquard miss the sprint with world champion Peter Sagan and Alexander Kristoff challenging Greipel for the win but ultimately coming up short.",
                    date: new DateTime(2016, 07, 24, 17, 53, 48),
                    authorUsername: "merry@gmail.com",
                    description: "In a stellar performance, Team Sky's Chris Froome finished Stage 21 as the winner of the 2016 Tour de France.",
                     tag: new List<Tag>()
                    {
                        Tour, News
                    },
                    comment: new List<Comment>()
                    {
                        new Comment(1, "One of the most boring tours I ve ever watched!!!", context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(1, "No one was half good as Chris.", context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(1, "All these young kids cry how hard things are. Poss off! Your life is easy. Ride and suffer."
                        , context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(1, "Froome running without bike was the best thing ever.", context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com")),
                    },
                    category: Road,
                    postPicture: tourPic
                );

                var giroPath = HttpContext.Current.Server.MapPath("~/Content/Images/Posts/Giro2016.jpg");
                Image giro = Image.FromFile(giroPath);
                byte[] giroPic = imgToByteArray(giro);
                CreatePost(context,
                  title: "The Winners of the 2016 Giro",
                  body: @"Vincenzo Nibali (Astana) secured victory at the 99th Giro d’Italia after finishing safely on the final stage of the race into Turin.
                    Giacomo Nizzolo (Trek-Segafredo) thought he had finally secured his first Grand Tour stage win after near 30 top-10 placings, but was 
                    relegated due to an irregular sprint with the win awarded to second place Nikia Arndt (Giant-Alpecin). Nizzolo still secured the red points jersey for a    second year in a row.
                    Nibali only had to finish the race to secure victory, after GC times were neutralised on the 8km finishing circuit with crashes taking place on the wet roads, including seventh place overall Rigoberto Uran (Cannondale) and runner-up Esteban Chaves (Orica-GreenEdge). Both were able to continue.
                    Unusually for the traditional processional affair of the final stage, at least three riders were forced to abandon so close to the end; Lars Bak (Lotto-Soudal) had to climb off after a crash in the very early part of the day, while Jasha Sütterlin (Movistar) and Johann Van Zyl (Dimension Data) 
                    also succumbed to injury having been caught in the crash with Uran.",
                  date: new DateTime(2016, 07, 24, 17, 53, 48),
                  authorUsername: "admin@gmail.com",
                  description: "The general classification was neutralised after Rigoberto Uran and Esteban Chaves was caught up in a crash with around " +
                               "30km to go, as provisional stage winner Giacomo Nizzolo was relegated for an irregular sprint",
                   tag: new List<Tag>()
                  {
                        Giro, News
                  },
                  comment: new List<Comment>()
                  {
                        new Comment(2, "Loved this Giro - every minute of it, and such a great advert for the sport. But so so sad to to see that, on the final podium, only 1 of the 3 has no history of cheating. Still a long way to go.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(2, "Nibali wins. The time he took off Kruiswijk I can see; 53 seconds off Chaves on stage 19 and then a further 96 on stage 20 - there should be a very serious contract awaiting at Team Bahrain (or what ever the name is to be). Another problem for Vino's in box then... Does seem odd that a man who can do this would have done that thing that got him disqualified from the Vuelta.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(2, "So Froome, Wiggins and Sky generally have to constantly answer questions and allegations about doping and Nibali is beyond suspicion? When his own team are blood testing him due to his lack of performance, then a day or two later it's the greatest comeback since Lazarus..... Surely even the most one eyed Nibali fan can see why questions might arise given the sports past (let alone the connections to Astana and Vino).", 
                        context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(2, "Can't feel good about the final result. From nowhere and done to winner... who trusts Astana? who trusts individual cyclists when- pride, money and.. money are at stake Who therefore trusts cycling, certainly not me after this result."
                        , context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com"))
                  },
                  category: Road,
                  postPicture: giroPic
              );

                var vaueltaPath = HttpContext.Current.Server.MapPath("~/Content/Images/Posts/Vauelta2015.jpg");
                Image vauelta = Image.FromFile(vaueltaPath);
                byte[] vaueltaPic = imgToByteArray(vauelta);
                CreatePost(context,
                  title: "The Winners of the 2015 Vauelta",
                  body: @"John Degenkolb (Giant-Alpecin) had to wait a long time for a Grand Tour stage win in 2015 but the German beat a depleted field in Madrid to seal his 10th Vuelta a España win.
                    Degenkolb looked to have gone too early in the final sprint but found a second wind to hold off Danny Van Poppel (Trek) and Jempy Drucker (BMC) on the line.
                    Fabio Aru (Astana) finished safely in the pack to win his first Grand Tour at only his fifth attempt.
                    After the traditional leisurely start to the stage, the real racing got going when the riders hit the circuit in Madrid.",
                  date: new DateTime(2016, 07, 24, 17, 53, 48),
                  authorUsername: "pesho@gmail.com",
                  description: "Fabio Aru finished the final stage of the Vuelta a España safely to seal his maiden Grand Tour win as John Degenkolb took the stage",
                   tag: new List<Tag>()
                  {
                        Vuelta, News
                  },
                  comment: new List<Comment>()
                  {
                        new Comment(3, "Loved this Giro - every minute of it, and such a great advert for the sport. But so so sad to to see that, on the final podium, only 1 of the 3 has no history of cheating. Still a long way to go.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(3, "Nibali wins. The time he took off Kruiswijk I can see; 53 seconds off Chaves on stage 19 and then a further 96 on stage 20 - there should be a very serious contract awaiting at Team Bahrain (or what ever the name is to be). Another problem for Vino's in box then... Does seem odd that a man who can do this would have done that thing that got him disqualified from the Vuelta.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(3, "So Froome, Wiggins and Sky generally have to constantly answer questions and allegations about doping and Nibali is beyond suspicion? When his own team are blood testing him due to his lack of performance, then a day or two later it's the greatest comeback since Lazarus..... Surely even the most one eyed Nibali fan can see why questions might arise given the sports past (let alone the connections to Astana and Vino).",
                        context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(3, "Can't feel good about the final result. From nowhere and done to winner... who trusts Astana? who trusts individual cyclists when- pride, money and.. money are at stake Who therefore trusts cycling, certainly not me after this result."
                        , context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com"))
                  },
                  category: Road,
                  postPicture: vaueltaPic
              );
                var mtbPath = HttpContext.Current.Server.MapPath("~/Content/Images/Posts/Mtb.jpg");
                Image mtb = Image.FromFile(mtbPath);
                byte[] mtbPic = imgToByteArray(mtb);
                CreatePost(context,
                  title: "Cycling UK wants to open up more trails to mountain bikers.",
                  body: @"To do this, Cycling UK wants to first gain a better understanding of how people's cycling habits fit in with current access arrangements to rights of way in England and Wales. By completing the organisation’s short survey, you can do your part to help it get a better picture of where, how and why people ride their bikes off road.
                        Under current access laws, cyclists have the right to use only a fraction of the country’s vast network of footpaths and bridleways.
                        Currently, whether or not a cyclist can use a right of way is determined by its historical usage as opposed to its suitability for riding. In practical terms, this means that a cyclist may have the right to freely skitter down a muddy and un-ridable bridleway but not on an asphalt surfaced footpath, even if the same path is used privately by motor vehicles.
                        Roger Geffe, Cycling UK’s policy director points towards Scotland’s common sense approach to access rights for cyclists and hopes that the same can be adopted in the future in the rest of the UK.
                        “We’ve seen how off-road cycling can thrive in harmony with all other outdoor users [in Scotland], and Cycling UK now wants to understand how we can bring the same benefits to England and Wales,” he says.
                        The survey can be found at: www.cyclinguk.org/offroad-survey. Every person who completes the survey will also be automatically entered into a prize draw to win an iPad mini.",
                  date: new DateTime(2016, 07, 24, 17, 53, 48),
                  authorUsername: "geshu@gmail.com",
                  description: "Cycling UK hopes to improve access rights for mountain bikers in England and Wales (Andy McCandlish).The popularity of " +
                               "mountain biking has exploded in the UK in recent years but little has been done to improve or clarify cyclists’ access arrangements " +
                               "to off-road paths and trails.",
                   tag: new List<Tag>()
                  {
                        Route, News
                  },
                  comment: new List<Comment>()
                  {
                        new Comment(4, "i'd love so much for footpaths and more pathways to be opened up, as most of the local roads are small with trucks, and people speeding and cutting corners, and it's annoying, as by me there's more footpaths than bridleways, and being allowed to ride on footpaths would open up around 100 miles of extra tracks to ride, and give direct routes to woods which have some trails.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(4, "I think this is people returning to cycling after many years & finding the roads too busy and dangerous. Especially country roads. After 6 months I left the A4 and headed for muddy bridleways and single track footpaths. I do 2x8miles twice per week to keep fit. The off-road world includes ramblers, dog walkers, cows \'n\' sheep, horse riders & other cyclists. On seeing any of these you must slow down because their behaviour is unpredictable. Apart from having to remain vigilant for these hazards it is better than being on the road. I use daytime front and rear LED lights, and a bell, so they can see & hear me coming.\r\n\r\nRecreational cyclists should be encouraged to leave the tarmac and the funerals behind. There have been too many of them. If you have to cycle to work then use the road - otherwise stick to the footpaths & bridleways - it is a different less confrontational kind of cycling.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com"))
                  },
                  category: Mountain,
                  postPicture: mtbPic
              );

                CreatePost(context,
                  title: "WIGGINS ON TRACK FOR RECORD MEDAL HAUL",
                  body: @"Wiggins will compete in the men’s team pursuit and can become the first British athlete to win eight Olympic medals. He has won six track cycling medals, including three golds, and a gold medal in the road cycling time trial, but Australia, New Zealand and Denmark all bring strong pursuiting squads to Rio 2016.
                        Led by Wiggins and now-retired Chris Hoy, the Great Britain team emerged as track cycling’s dominant force in 2008, and have won seven gold medals at each of the past two Olympic Games. The British will be the team to beat as men and women each race for five gold medals in three sprint events (sprint, team sprint and keirin) and two endurance events (team pursuit and omnium).",
                  date: new DateTime(2016, 07, 24, 17, 53, 48),
                  authorUsername: "geshu@gmail.com",
                  description: "BRADLEY WIGGINS CAN CLAIM A RECORD EIGHTH OLYMPIC MEDAL BUT GREAT BRITAIN’S EIGHT-YEAR DOMINANCE OF OLYMPIC TRACK CYCLING FACES A STIFF CHALLENGE WHEN COMPETITION BEGINS AT THE NEW RIO OLYMPIC VELODROME ON 11 AUGUST.",
                   tag: new List<Tag>()
                  {
                        Olympic, News
                  },
                  comment: new List<Comment>()
                  {
                        new Comment(5, "i'd love so much for footpaths and more pathways to be opened up, as most of the local roads are small with trucks, and people speeding and cutting corners, and it's annoying, as by me there's more footpaths than bridleways, and being allowed to ride on footpaths would open up around 100 miles of extra tracks to ride, and give direct routes to woods which have some trails.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(5, "I think this is people returning to cycling after many years & finding the roads too busy and dangerous. Especially country roads. After 6 months I left the A4 and headed for muddy bridleways and single track footpaths. I do 2x8miles twice per week to keep fit. The off-road world includes ramblers, dog walkers, cows \'n\' sheep, horse riders & other cyclists. On seeing any of these you must slow down because their behaviour is unpredictable. Apart from having to remain vigilant for these hazards it is better than being on the road. I use daytime front and rear LED lights, and a bell, so they can see & hear me coming.\r\n\r\nRecreational cyclists should be encouraged to leave the tarmac and the funerals behind. There have been too many of them. If you have to cycle to work then use the road - otherwise stick to the footpaths & bridleways - it is a different less confrontational kind of cycling.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(5, "Loved this Giro - every minute of it, and such a great advert for the sport. But so so sad to to see that, on the final podium, only 1 of the 3 has no history of cheating. Still a long way to go.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(5, "Nibali wins. The time he took off Kruiswijk I can see; 53 seconds off Chaves on stage 19 and then a further 96 on stage 20 - there should be a very serious contract awaiting at Team Bahrain (or what ever the name is to be). Another problem for Vino's in box then... Does seem odd that a man who can do this would have done that thing that got him disqualified from the Vuelta.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(5, "So Froome, Wiggins and Sky generally have to constantly answer questions and allegations about doping and Nibali is beyond suspicion? When his own team are blood testing him due to his lack of performance, then a day or two later it's the greatest comeback since Lazarus..... Surely even the most one eyed Nibali fan can see why questions might arise given the sports past (let alone the connections to Astana and Vino).",
                        context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(5, "Can't feel good about the final result. From nowhere and done to winner... who trusts Astana? who trusts individual cyclists when- pride, money and.. money are at stake Who therefore trusts cycling, certainly not me after this result."
                        , context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com")),
                        new Comment(5, "Loved this Giro - every minute of it, and such a great advert for the sport. But so so sad to to see that, on the final podium, only 1 of the 3 has no history of cheating. Still a long way to go.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(5, "Nibali wins. The time he took off Kruiswijk I can see; 53 seconds off Chaves on stage 19 and then a further 96 on stage 20 - there should be a very serious contract awaiting at Team Bahrain (or what ever the name is to be). Another problem for Vino's in box then... Does seem odd that a man who can do this would have done that thing that got him disqualified from the Vuelta.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(5, "So Froome, Wiggins and Sky generally have to constantly answer questions and allegations about doping and Nibali is beyond suspicion? When his own team are blood testing him due to his lack of performance, then a day or two later it's the greatest comeback since Lazarus..... Surely even the most one eyed Nibali fan can see why questions might arise given the sports past (let alone the connections to Astana and Vino).",
                        context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(5, "Can't feel good about the final result. From nowhere and done to winner... who trusts Astana? who trusts individual cyclists when- pride, money and.. money are at stake Who therefore trusts cycling, certainly not me after this result."
                        , context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com")),
                        new Comment(5, "Loved this Giro - every minute of it, and such a great advert for the sport. But so so sad to to see that, on the final podium, only 1 of the 3 has no history of cheating. Still a long way to go.",
                        context.Users.FirstOrDefault(u => u.UserName == "merry@gmail.com")),
                        new Comment(5, "Nibali wins. The time he took off Kruiswijk I can see; 53 seconds off Chaves on stage 19 and then a further 96 on stage 20 - there should be a very serious contract awaiting at Team Bahrain (or what ever the name is to be). Another problem for Vino's in box then... Does seem odd that a man who can do this would have done that thing that got him disqualified from the Vuelta.",
                        context.Users.FirstOrDefault(u => u.UserName == "pesho@gmail.com")),
                        new Comment(5, "So Froome, Wiggins and Sky generally have to constantly answer questions and allegations about doping and Nibali is beyond suspicion? When his own team are blood testing him due to his lack of performance, then a day or two later it's the greatest comeback since Lazarus..... Surely even the most one eyed Nibali fan can see why questions might arise given the sports past (let alone the connections to Astana and Vino).",
                        context.Users.FirstOrDefault(u => u.UserName == "geshu@gmail.com")),
                        new Comment(5, "Can't feel good about the final result. From nowhere and done to winner... who trusts Astana? who trusts individual cyclists when- pride, money and.. money are at stake Who therefore trusts cycling, certainly not me after this result."
                        , context.Users.FirstOrDefault(u => u.UserName == "admin@gmail.com"))

                  },
                  category: Track,
                  postPicture: null
              );

                context.SaveChanges();
            }
        }

        private void CreateUser(ApplicationDbContext context,
            string email, string password, string fullName, byte[] profilePicture)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };
            user.ProfilePicture = profilePicture;
          var userCreateResult = userManager.Create(user, password);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(roleName));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private void AddUserToRole(ApplicationDbContext context, string userName, string roleName)
        {
            var user = context.Users.First(u => u.UserName == userName);
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var addAdminRoleResult = userManager.AddToRole(user.Id, roleName);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreatePost(ApplicationDbContext context,
            string title, string body, DateTime date, string authorUsername, string description, List<Tag> tag, List<Comment> comment, Category category, byte[] postPicture)
        {
            var post = new Post();
            post.Title = title;
            post.Body = body;
            post.Date = date;
            post.Author = context.Users.FirstOrDefault(u => u.UserName == authorUsername);
            post.Description = description;
            post.Tags = tag;
            post.Comment = comment;
            post.Category = category;
            post.PostPicture = postPicture;
            context.Posts.Add(post);
        }

        private byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }




    }
}