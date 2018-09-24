using System.Data.Entity;
using Oakinstream.Models;

namespace Oakinstream.DAL
{
    public class OakinstreamContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCategoryModels> ProjectCategorys { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<ProjectFileMapping> ProjectFileMappins { get; set; }

        public DbSet<BlogModels> Blogs { get; set; }
        public DbSet<BlogCategoryModels> BlogCategorys { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<BlogImageMapping> BlogImageMappings { get; set; }

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
    }
}