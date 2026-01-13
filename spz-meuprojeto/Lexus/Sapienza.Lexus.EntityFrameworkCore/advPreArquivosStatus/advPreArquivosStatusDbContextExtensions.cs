using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreArquivosStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreArquivosStatus(this ModelBuilder builder)
    {
        builder.Entity<advPreArquivosStatus>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreArquivosStatuses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
