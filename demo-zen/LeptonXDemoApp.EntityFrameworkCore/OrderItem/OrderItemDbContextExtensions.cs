using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class OrderItemDbContextModelCreatingExtensions
{
    public static void ConfigureOrderItem(this ModelBuilder builder)
    {
        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "OrderItems", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Order>()
                .WithMany(p => p.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
