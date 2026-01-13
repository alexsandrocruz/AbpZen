using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreOrigensDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreOrigens(this ModelBuilder builder)
    {
        builder.Entity<advPreOrigens>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreOrigenses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
