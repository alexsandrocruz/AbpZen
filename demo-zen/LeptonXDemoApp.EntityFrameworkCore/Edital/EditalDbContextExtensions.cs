using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class EditalDbContextModelCreatingExtensions
{
    public static void ConfigureEdital(this ModelBuilder builder)
    {
        builder.Entity<Edital>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Editais", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
