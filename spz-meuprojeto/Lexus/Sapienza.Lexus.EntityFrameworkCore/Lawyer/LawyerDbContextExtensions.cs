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

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
