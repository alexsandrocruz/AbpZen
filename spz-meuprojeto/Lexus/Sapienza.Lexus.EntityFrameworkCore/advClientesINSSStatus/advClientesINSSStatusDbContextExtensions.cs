using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesINSSStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesINSSStatus(this ModelBuilder builder)
    {
        builder.Entity<advClientesINSSStatus>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientesINSSStatuses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientesINSS>()
                .WithMany(p => p.advClientesINSSes)
                .HasForeignKey(x => x.advClientesINSSId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
