using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fdtDevsDbContextModelCreatingExtensions
{
    public static void ConfigurefdtDevs(this ModelBuilder builder)
    {
        builder.Entity<fdtDevs>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fdtDevses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
