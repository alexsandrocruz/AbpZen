using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class CustomFieldDbContextModelCreatingExtensions
{
    public static void ConfigureCustomField(this ModelBuilder builder)
    {
        builder.Entity<CustomField>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "CustomFields", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.EntityType).IsRequired();
            b.Property(x => x.Label).IsRequired();
            b.Property(x => x.FieldType).IsRequired();
            b.Property(x => x.FieldKey).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
