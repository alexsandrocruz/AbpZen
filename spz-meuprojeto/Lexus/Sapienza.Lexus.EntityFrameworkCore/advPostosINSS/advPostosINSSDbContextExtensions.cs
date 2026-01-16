using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPostosINSSDbContextModelCreatingExtensions
{
    public static void ConfigureadvPostosINSS(this ModelBuilder builder)
    {
        builder.Entity<advPostosINSS>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPostosINSSes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientes>()
                .WithMany(p => p.advClienteses)
                .HasForeignKey(x => x.advClientesId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
