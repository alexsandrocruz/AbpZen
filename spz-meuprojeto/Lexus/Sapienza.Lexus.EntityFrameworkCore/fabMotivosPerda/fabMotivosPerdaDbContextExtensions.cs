using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabMotivosPerdaDbContextModelCreatingExtensions
{
    public static void ConfigurefabMotivosPerda(this ModelBuilder builder)
    {
        builder.Entity<fabMotivosPerda>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabMotivosPerdas", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
