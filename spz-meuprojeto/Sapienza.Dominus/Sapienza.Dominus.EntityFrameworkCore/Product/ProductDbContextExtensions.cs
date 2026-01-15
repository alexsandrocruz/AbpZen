using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProductDbContextModelCreatingExtensions
{
    public static void ConfigureProduct(this ModelBuilder builder)
    {
        builder.Entity<Product>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Products", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
