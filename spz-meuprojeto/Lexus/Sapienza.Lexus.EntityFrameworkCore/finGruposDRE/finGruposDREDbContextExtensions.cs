using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finGruposDREDbContextModelCreatingExtensions
{
    public static void ConfigurefinGruposDRE(this ModelBuilder builder)
    {
        builder.Entity<finGruposDRE>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finGruposDREs", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
