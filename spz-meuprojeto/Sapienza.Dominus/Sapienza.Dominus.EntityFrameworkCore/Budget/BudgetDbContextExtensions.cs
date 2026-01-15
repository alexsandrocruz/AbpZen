using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class BudgetDbContextModelCreatingExtensions
{
    public static void ConfigureBudget(this ModelBuilder builder)
    {
        builder.Entity<Budget>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Budgets", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<FinancialCategory>()
                .WithMany(p => p.Budgets)
                .HasForeignKey(x => x.FinancialCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
