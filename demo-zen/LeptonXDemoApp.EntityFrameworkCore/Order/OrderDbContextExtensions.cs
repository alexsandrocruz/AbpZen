using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class OrderDbContextModelCreatingExtensions
{
    public static void ConfigureOrder(this ModelBuilder builder)
    {
        builder.Entity<Order>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Orders", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Customer>()
                .WithMany(p => p.Orders)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
