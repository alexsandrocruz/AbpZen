using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class BlogCategoryDbContextModelCreatingExtensions
{
    public static void ConfigureBlogCategory(this ModelBuilder builder)
    {
        builder.Entity<BlogCategory>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "BlogCategories", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
