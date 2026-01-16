using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesConvertidosDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesConvertidos(this ModelBuilder builder)
    {
        builder.Entity<advClientesConvertidos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientesConvertidoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.data).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
