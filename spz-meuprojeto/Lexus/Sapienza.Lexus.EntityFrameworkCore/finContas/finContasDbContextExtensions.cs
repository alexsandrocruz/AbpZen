using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finContasDbContextModelCreatingExtensions
{
    public static void ConfigurefinContas(this ModelBuilder builder)
    {
        builder.Entity<finContas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finContases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
