using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class BookingDbContextModelCreatingExtensions
{
    public static void ConfigureBooking(this ModelBuilder builder)
    {
        builder.Entity<Booking>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Bookings", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ClientName).IsRequired();
            b.Property(x => x.ClientEmail).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<SchedulerType>()
                .WithMany(p => p.Bookings)
                .HasForeignKey(x => x.SchedulerTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<Client>()
                .WithMany(p => p.Bookings)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
