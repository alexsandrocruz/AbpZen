using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCompromissosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCompromissos(this ModelBuilder builder)
    {
        builder.Entity<advCompromissos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCompromissoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
