using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advTarefasAtualizacoesDbContextModelCreatingExtensions
{
    public static void ConfigureadvTarefasAtualizacoes(this ModelBuilder builder)
    {
        builder.Entity<advTarefasAtualizacoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advTarefasAtualizacoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
