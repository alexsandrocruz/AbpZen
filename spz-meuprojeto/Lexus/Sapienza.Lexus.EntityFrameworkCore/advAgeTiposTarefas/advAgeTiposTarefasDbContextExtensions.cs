using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advAgeTiposTarefasDbContextModelCreatingExtensions
{
    public static void ConfigureadvAgeTiposTarefas(this ModelBuilder builder)
    {
        builder.Entity<advAgeTiposTarefas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advAgeTiposTarefases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
