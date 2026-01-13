using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class EventCommissionDbContextModelCreatingExtensions
{
    public static void ConfigureEventCommission(this ModelBuilder builder)
    {
        builder.Entity<EventCommission>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "EventCommissions", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Event>()
                .WithMany(p => p.EventCommissions)
                .HasForeignKey(x => x.EventId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
