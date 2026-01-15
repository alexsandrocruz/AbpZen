using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ClientContactDbContextModelCreatingExtensions
{
    public static void ConfigureClientContact(this ModelBuilder builder)
    {
        builder.Entity<ClientContact>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ClientContacts", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.ClientContacts)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
