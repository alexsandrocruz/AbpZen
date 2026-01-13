using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advTarefasDbContextModelCreatingExtensions
{
    public static void ConfigureadvTarefas(this ModelBuilder builder)
    {
        builder.Entity<advTarefas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advTarefases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
