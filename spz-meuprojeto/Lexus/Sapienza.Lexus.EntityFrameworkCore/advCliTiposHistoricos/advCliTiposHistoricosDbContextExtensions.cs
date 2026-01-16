using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliTiposHistoricosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliTiposHistoricos(this ModelBuilder builder)
    {
        builder.Entity<advCliTiposHistoricos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliTiposHistoricoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientesHistoricos>()
                .WithMany(p => p.advClientesHistoricoses)
                .HasForeignKey(x => x.advClientesHistoricosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
