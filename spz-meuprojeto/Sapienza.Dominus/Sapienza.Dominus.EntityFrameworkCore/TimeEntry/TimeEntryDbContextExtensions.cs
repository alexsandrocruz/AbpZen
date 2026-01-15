using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class TimeEntryDbContextModelCreatingExtensions
{
    public static void ConfigureTimeEntry(this ModelBuilder builder)
    {
        builder.Entity<TimeEntry>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "TimeEntries", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Description).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Project>()
                .WithMany(p => p.TimeEntries)
                .HasForeignKey(x => x.ProjectId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
