using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ClientMessageDbContextModelCreatingExtensions
{
    public static void ConfigureClientMessage(this ModelBuilder builder)
    {
        builder.Entity<ClientMessage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ClientMessages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Channel).IsRequired();
            b.Property(x => x.Direction).IsRequired();
            b.Property(x => x.Content).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.ClientMessages)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
