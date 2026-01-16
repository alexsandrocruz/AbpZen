using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advAgeTiposCompromissosDbContextModelCreatingExtensions
{
    public static void ConfigureadvAgeTiposCompromissos(this ModelBuilder builder)
    {
        builder.Entity<advAgeTiposCompromissos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advAgeTiposCompromissoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advCompromissos>()
                .WithMany(p => p.advCompromissoses)
                .HasForeignKey(x => x.advCompromissosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
