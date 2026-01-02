using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class CategoryDbContextModelCreatingExtensions
{
    public static void ConfigureCategory(this ModelBuilder builder)
    {
        builder.Entity<Category>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Categories", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
