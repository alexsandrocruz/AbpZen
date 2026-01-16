using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosDadosHerdeirosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosDadosHerdeiros(this ModelBuilder builder)
    {
        builder.Entity<advProcessosDadosHerdeiros>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProcessosDadosHerdeiroses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
