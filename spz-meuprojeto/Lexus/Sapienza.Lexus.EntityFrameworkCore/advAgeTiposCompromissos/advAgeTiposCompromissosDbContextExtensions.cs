using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advAgeTiposCompromissosDbContextModelCreatingExtensions
{
    public static void ConfigureadvAgeTiposCompromissos(this ModelBuilder builder)
    {
        builder.Entity<advAgeTiposCompromissos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advAgeTiposCompromissoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
