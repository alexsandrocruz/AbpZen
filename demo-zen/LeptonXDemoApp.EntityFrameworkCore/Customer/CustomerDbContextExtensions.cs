using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class CustomerDbContextModelCreatingExtensions
{
    public static void ConfigureCustomer(this ModelBuilder builder)
    {
        builder.Entity<Customer>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Customers", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
