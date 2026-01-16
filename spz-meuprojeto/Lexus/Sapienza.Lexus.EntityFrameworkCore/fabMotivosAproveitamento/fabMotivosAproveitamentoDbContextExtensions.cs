using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabMotivosAproveitamentoDbContextModelCreatingExtensions
{
    public static void ConfigurefabMotivosAproveitamento(this ModelBuilder builder)
    {
        builder.Entity<fabMotivosAproveitamento>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabMotivosAproveitamentos", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
