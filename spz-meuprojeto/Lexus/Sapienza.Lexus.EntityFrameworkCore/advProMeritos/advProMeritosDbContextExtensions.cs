using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProMeritosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProMeritos(this ModelBuilder builder)
    {
        builder.Entity<advProMeritos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProMeritoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advProcessosMeritos>()
                .WithMany(p => p.advProcessosMeritoses)
                .HasForeignKey(x => x.advProcessosMeritosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
