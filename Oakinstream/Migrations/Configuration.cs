using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Oakinstream.Models;
using Oakinstream.Services;

namespace Oakinstream.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Oakinstream.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Oakinstream.Models.ApplicationDbContext";
        }

        protected override void Seed(Oakinstream.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            if (!context.Users.Any(t => t.UserName == "admin@mvcatm.com"))
            {
                var user = new ApplicationUser { UserName = "admin@mvcatm.com", Email = "admin@mvcatm.com" };
                userManager.Create(user, "passW0rd!");

                var service = new CheckingAccountService(context);
                service.CreateCheckingAccount("admin", "user", user.Id);

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
            }

            var projectCategory = new List<ProjectCategory>
            {
                new ProjectCategory {Name = "projectCategory1"},
                new ProjectCategory {Name = "projectCategory2"},
                new ProjectCategory {Name = "projectCategory3"},
                new ProjectCategory {Name = "projectCategory4"},
            };
            projectCategory.ForEach(c => context.ProjectCategorys.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();


            var blogCategory = new List<BlogCategory>
            {
                new BlogCategory {Name = "projectCategory1"},
                new BlogCategory {Name = "projectCategory2"},
                new BlogCategory {Name = "projectCategory3"},
                new BlogCategory {Name = "projectCategory4"},
            };
            blogCategory.ForEach(c => context.BlogCategorys.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            if (!context.Abouts.Any(t => t.ID == 1))
            {
                var Defualtinfo = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut porttitor leo a diam sollicitudin tempor id eu nisl. Malesuada fames ac turpis egestas maecenas pharetra convallis.";
                var about = new List<About>
                {
                    new About
                    {
                    ID = 1,
                    Name = "Your Name",
                    Age = 100,
                    AboutImageID = null,
                    Info1 = Defualtinfo,
                    Info2 = Defualtinfo,
                    Info3 = Defualtinfo,
                    CreatedBy = "Default",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = DateTime.Now
                    }
                };
                about.ForEach(c => context.Abouts.AddOrUpdate(p => p.Name, c));
                context.SaveChanges();
            }


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
