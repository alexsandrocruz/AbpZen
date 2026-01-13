using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosDadosHerdeirosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosDadosHerdeiros(this ModelBuilder builder)
    {
        builder.Entity<advProcessosDadosHerdeiros>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProcessosDadosHerdeiroses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
