using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class TransactionDbContextModelCreatingExtensions
{
    public static void ConfigureTransaction(this ModelBuilder builder)
    {
        builder.Entity<Transaction>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Transactions", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Description).IsRequired();
            b.Property(x => x.Type).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Transactions)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<FinancialCategory>()
                .WithMany(p => p.Transactions)
                .HasForeignKey(x => x.FinancialCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
