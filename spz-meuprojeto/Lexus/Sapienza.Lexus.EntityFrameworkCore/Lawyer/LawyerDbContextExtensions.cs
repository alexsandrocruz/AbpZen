using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class LawyerDbContextModelCreatingExtensions
{
    public static void ConfigureLawyer(this ModelBuilder builder)
    {
        builder.Entity<Lawyer>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "Lawyers", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).HasMaxLength(128);
            b.Property(x => x.FullName).IsRequired();
            b.Property(x => x.PreferredName).HasMaxLength(64);

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
