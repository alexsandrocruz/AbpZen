using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProjectDbContextModelCreatingExtensions
{
    public static void ConfigureProject(this ModelBuilder builder)
    {
        builder.Entity<Project>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Projects", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Projects)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
