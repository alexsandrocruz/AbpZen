using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advVerTiposDbContextModelCreatingExtensions
{
    public static void ConfigureadvVerTipos(this ModelBuilder builder)
    {
        builder.Entity<advVerTipos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advVerTiposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advVerbas>()
                .WithMany(p => p.advVerbases)
                .HasForeignKey(x => x.advVerbasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
