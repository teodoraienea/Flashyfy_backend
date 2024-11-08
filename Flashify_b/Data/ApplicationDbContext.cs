using Microsoft.EntityFrameworkCore;
using Flashify_b.Models;

namespace Flashify_b.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlashCard>()
                .HasOne(f => f.User)
                .WithMany() 
                .HasForeignKey(f => f.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
