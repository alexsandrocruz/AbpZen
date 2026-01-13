using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabFormasRecebimentoDbContextModelCreatingExtensions
{
    public static void ConfigurefabFormasRecebimento(this ModelBuilder builder)
    {
        builder.Entity<fabFormasRecebimento>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabFormasRecebimentos", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
