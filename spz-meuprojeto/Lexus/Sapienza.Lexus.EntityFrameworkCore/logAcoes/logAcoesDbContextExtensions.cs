using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class logAcoesDbContextModelCreatingExtensions
{
    public static void ConfigurelogAcoes(this ModelBuilder builder)
    {
        builder.Entity<logAcoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "logAcoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<logCampos>()
                .WithMany(p => p.logCamposes)
                .HasForeignKey(x => x.logCamposId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
