using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsPortal.Persistence
{
    public class NewsPortalContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public NewsPortalContext(DbContextOptions<NewsPortalContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            // A felhasználói tábla alapértelemezett neve AspNetUsers lenne az adatbázisban, de ezt felüldefiniálhatjuk.
        }

       
        public DbSet<Article> Articles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }
}
