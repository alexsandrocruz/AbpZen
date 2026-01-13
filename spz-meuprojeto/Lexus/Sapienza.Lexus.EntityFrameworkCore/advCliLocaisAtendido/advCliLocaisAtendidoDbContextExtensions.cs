using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliLocaisAtendidoDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliLocaisAtendido(this ModelBuilder builder)
    {
        builder.Entity<advCliLocaisAtendido>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliLocaisAtendidos", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
