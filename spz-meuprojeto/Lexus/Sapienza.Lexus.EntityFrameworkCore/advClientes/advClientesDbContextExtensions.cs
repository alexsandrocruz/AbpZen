using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientes(this ModelBuilder builder)
    {
        builder.Entity<advClientes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advClienteses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
