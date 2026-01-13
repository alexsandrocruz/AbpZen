using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosClientesDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosClientes(this ModelBuilder builder)
    {
        builder.Entity<advProcessosClientes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProcessosClienteses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
