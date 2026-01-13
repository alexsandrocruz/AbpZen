using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class logAcoesDbContextModelCreatingExtensions
{
    public static void ConfigurelogAcoes(this ModelBuilder builder)
    {
        builder.Entity<logAcoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "logAcoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
