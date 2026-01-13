using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesAtualizacoesDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesAtualizacoes(this ModelBuilder builder)
    {
        builder.Entity<advClientesAtualizacoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advClientesAtualizacoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
