using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class BlogPostVersionDbContextModelCreatingExtensions
{
    public static void ConfigureBlogPostVersion(this ModelBuilder builder)
    {
        builder.Entity<BlogPostVersion>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "BlogPostVersions", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<BlogPost>()
                .WithMany(p => p.BlogPostVersions)
                .HasForeignKey(x => x.BlogPostId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
