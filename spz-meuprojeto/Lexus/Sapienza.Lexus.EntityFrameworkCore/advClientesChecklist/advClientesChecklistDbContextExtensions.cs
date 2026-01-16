using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesChecklistDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesChecklist(this ModelBuilder builder)
    {
        builder.Entity<advClientesChecklist>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientesChecklists", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
