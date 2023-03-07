using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<BlogPostModel> BlogPosts { get; set; }
        public DbSet<UrlPair> urlPairs { get; set; }
        public DbSet<ContactModel> messages { get; set; }
    }
}
