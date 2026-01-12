using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class SpecializationDbContextModelCreatingExtensions
{
    public static void ConfigureSpecialization(this ModelBuilder builder)
    {
        builder.Entity<Specialization>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "Specializations", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(64);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Description).HasMaxLength(512);

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
