using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class autoFTPDbContextModelCreatingExtensions
{
    public static void ConfigureautoFTP(this ModelBuilder builder)
    {
        builder.Entity<autoFTP>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "autoFTPs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
