using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class FileDbContextModelCreatingExtensions
{
    public static void ConfigureFile(this ModelBuilder builder)
    {
        builder.Entity<File>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Files", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FileName).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
