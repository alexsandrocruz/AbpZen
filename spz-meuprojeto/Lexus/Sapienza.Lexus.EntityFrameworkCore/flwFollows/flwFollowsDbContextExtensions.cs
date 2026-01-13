using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwFollowsDbContextModelCreatingExtensions
{
    public static void ConfigureflwFollows(this ModelBuilder builder)
    {
        builder.Entity<flwFollows>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "flwFollowses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.data).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
