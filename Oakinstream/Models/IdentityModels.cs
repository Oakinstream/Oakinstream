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

        public System.Data.Entity.DbSet<Oakinstream.Models.ProjectCategoryModels> ProjectCategoryModels { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.ProjectModels> ProjectModels { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.BlogModels> BlogModels { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.BlogImage> BlogImages { get; set; }
        public DbSet<BlogImageMapping> BlogImageMappings { get; set; }

        public System.Data.Entity.DbSet<Oakinstream.Models.BlogCategoryModels> BlogCategoryModels { get; set; }
    }
}