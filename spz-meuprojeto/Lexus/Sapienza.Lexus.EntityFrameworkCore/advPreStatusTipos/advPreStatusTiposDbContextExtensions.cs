using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreStatusTiposDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreStatusTipos(this ModelBuilder builder)
    {
        builder.Entity<advPreStatusTipos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreStatusTiposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advPreStatus>()
                .WithMany(p => p.advPreStatuses)
                .HasForeignKey(x => x.advPreStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
