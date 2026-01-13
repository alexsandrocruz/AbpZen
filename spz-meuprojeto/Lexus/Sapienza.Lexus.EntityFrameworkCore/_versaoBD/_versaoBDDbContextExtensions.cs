using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class _versaoBDDbContextModelCreatingExtensions
{
    public static void Configure_versaoBD(this ModelBuilder builder)
    {
        builder.Entity<_versaoBD>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "_versaoBDs", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
