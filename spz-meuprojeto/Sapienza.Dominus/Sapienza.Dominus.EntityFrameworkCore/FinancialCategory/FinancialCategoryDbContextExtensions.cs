using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class FinancialCategoryDbContextModelCreatingExtensions
{
    public static void ConfigureFinancialCategory(this ModelBuilder builder)
    {
        builder.Entity<FinancialCategory>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "FinancialCategories", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Type).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
