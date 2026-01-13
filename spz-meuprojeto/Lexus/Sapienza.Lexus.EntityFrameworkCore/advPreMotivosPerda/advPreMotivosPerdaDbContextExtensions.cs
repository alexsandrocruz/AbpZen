using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreMotivosPerdaDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreMotivosPerda(this ModelBuilder builder)
    {
        builder.Entity<advPreMotivosPerda>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreMotivosPerdas", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
