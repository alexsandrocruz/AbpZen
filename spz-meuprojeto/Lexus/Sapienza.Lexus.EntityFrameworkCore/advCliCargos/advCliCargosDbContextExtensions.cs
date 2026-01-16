using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliCargosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliCargos(this ModelBuilder builder)
    {
        builder.Entity<advCliCargos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliCargoses", LexusConsts.DbSchema);
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
