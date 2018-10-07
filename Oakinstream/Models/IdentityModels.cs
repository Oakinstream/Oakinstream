using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Oakinstream.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Pin { get; set; }
    }

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

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.Project> Projects { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.ProjectCategory> ProjectCategorys { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<ProjectFileMapping> ProjectFileMappings { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.ProjectImage> ProjectImages { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.Blog> Blogs { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.BlogCategory> BlogCategorys { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.BlogImage> BlogImages { get; set; }
        public DbSet<BlogImageMapping> BlogImageMappings { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.About> Abouts { get; set; }
        public DbSet<AboutFile> AboutFiles { get; set; }
        public DbSet<AboutFileMapping> AboutFileMappings { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.AboutImage> AboutImages { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.Contact> Contacts { get; set; }
        public System.Data.Entity.DbSet<Oakinstream.Models.HomeImage> HomeImages { get; set; }
        public DbSet<HomeImageMapping> HomeImageMappings { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.Home> Homes { get; set; }
    }
}