using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class EmailLogDbContextModelCreatingExtensions
{
    public static void ConfigureEmailLog(this ModelBuilder builder)
    {
        builder.Entity<EmailLog>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "EmailLogs", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ToEmail).IsRequired();
            b.Property(x => x.Subject).IsRequired();
            b.Property(x => x.Status).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
