using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finContasClientesDbContextModelCreatingExtensions
{
    public static void ConfigurefinContasClientes(this ModelBuilder builder)
    {
        builder.Entity<finContasClientes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finContasClienteses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
