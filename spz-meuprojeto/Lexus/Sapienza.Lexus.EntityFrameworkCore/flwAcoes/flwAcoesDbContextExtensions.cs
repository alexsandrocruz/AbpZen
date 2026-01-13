using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwAcoesDbContextModelCreatingExtensions
{
    public static void ConfigureflwAcoes(this ModelBuilder builder)
    {
        builder.Entity<flwAcoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "flwAcoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
