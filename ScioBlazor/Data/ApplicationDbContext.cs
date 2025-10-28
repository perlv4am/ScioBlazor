using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ScioBlazor.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ShareLink> ShareLinks => Set<ShareLink>();
        public DbSet<Meeting> Meetings => Set<Meeting>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShareLink>(b =>
            {
                b.HasIndex(x => x.Token).IsUnique();
                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Meeting>(b =>
            {
                b.HasIndex(x => new { x.OwnerId, x.StartUtc });
                b.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne<ShareLink>()
                    .WithMany()
                    .HasForeignKey(x => x.ShareLinkId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(x => x.ShareLinkId)
                    .IsUnique()
                    .HasFilter("[ShareLinkId] IS NOT NULL");
            });
        }
    }
}
