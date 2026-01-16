using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advAgeTiposTarefasDbContextModelCreatingExtensions
{
    public static void ConfigureadvAgeTiposTarefas(this ModelBuilder builder)
    {
        builder.Entity<advAgeTiposTarefas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advAgeTiposTarefases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advTarefas>()
                .WithMany(p => p.advTarefases)
                .HasForeignKey(x => x.advTarefasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
