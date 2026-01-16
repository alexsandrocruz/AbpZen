using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreMotivosPerdaDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreMotivosPerda(this ModelBuilder builder)
    {
        builder.Entity<advPreMotivosPerda>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreMotivosPerdas", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
