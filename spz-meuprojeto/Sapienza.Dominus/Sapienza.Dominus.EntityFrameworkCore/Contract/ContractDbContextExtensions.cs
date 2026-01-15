using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ContractDbContextModelCreatingExtensions
{
    public static void ConfigureContract(this ModelBuilder builder)
    {
        builder.Entity<Contract>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Contracts", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Contracts)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
