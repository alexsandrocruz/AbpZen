using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliSituacoesDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliSituacoes(this ModelBuilder builder)
    {
        builder.Entity<advCliSituacoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliSituacoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientes>()
                .WithMany(p => p.advClienteses)
                .HasForeignKey(x => x.advClientesId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
