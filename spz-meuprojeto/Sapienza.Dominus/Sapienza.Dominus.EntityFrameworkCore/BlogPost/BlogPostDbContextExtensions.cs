using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class BlogPostDbContextModelCreatingExtensions
{
    public static void ConfigureBlogPost(this ModelBuilder builder)
    {
        builder.Entity<BlogPost>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "BlogPosts", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Slug).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Site>()
                .WithMany(p => p.BlogPosts)
                .HasForeignKey(x => x.SiteId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
