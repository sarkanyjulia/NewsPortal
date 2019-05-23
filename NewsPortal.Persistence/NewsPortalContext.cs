using System;
using Microsoft.EntityFrameworkCore;

namespace NewsPortal.Persistence
{
    public class NewsPortalContext : DbContext
    {
        public NewsPortalContext(DbContextOptions<NewsPortalContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
    }
}
