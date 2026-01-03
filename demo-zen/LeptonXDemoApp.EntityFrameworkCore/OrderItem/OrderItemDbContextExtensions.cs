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
            b.HasOne<Product>()
                .WithMany(p => p.OrderItems)
                .HasForeignKey(x => x.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<Order>()
                .WithMany(p => p.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
