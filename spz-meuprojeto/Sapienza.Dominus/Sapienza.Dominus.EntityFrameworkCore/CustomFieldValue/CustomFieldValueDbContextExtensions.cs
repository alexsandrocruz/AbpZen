using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class CustomFieldValueDbContextModelCreatingExtensions
{
    public static void ConfigureCustomFieldValue(this ModelBuilder builder)
    {
        builder.Entity<CustomFieldValue>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "CustomFieldValues", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.EntityId).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<CustomField>()
                .WithMany(p => p.CustomFieldValues)
                .HasForeignKey(x => x.CustomFieldId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
