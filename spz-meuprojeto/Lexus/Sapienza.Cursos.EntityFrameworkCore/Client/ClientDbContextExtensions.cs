using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class ClientDbContextModelCreatingExtensions
{
    public static void ConfigureClient(this ModelBuilder builder)
    {
        builder.Entity<Client>(b =>
        {
            b.ToTable(Sapienza.CursosConsts.DbTablePrefix + "Clients", Sapienza.CursosConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(128);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Email).HasMaxLength(128);
            b.Property(x => x.Email).IsRequired();
            b.Property(x => x.Phone).HasMaxLength(20);
            b.Property(x => x.CpfCnpj).HasMaxLength(20);
            b.Property(x => x.CpfCnpj).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
