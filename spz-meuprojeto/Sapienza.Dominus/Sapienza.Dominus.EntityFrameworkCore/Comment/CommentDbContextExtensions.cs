using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class CommentDbContextModelCreatingExtensions
{
    public static void ConfigureComment(this ModelBuilder builder)
    {
        builder.Entity<Comment>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Comments", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.EntityType).IsRequired();
            b.Property(x => x.Content).IsRequired();
            b.Property(x => x.EntityId).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
