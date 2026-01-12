using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class LawyerSpecializationDbContextModelCreatingExtensions
{
    public static void ConfigureLawyerSpecialization(this ModelBuilder builder)
    {
        builder.Entity<LawyerSpecialization>(b =>
        {
            b.ToTable(Sapienza.CursosConsts.DbTablePrefix + "LawyerSpecializations", Sapienza.CursosConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Lawyer>()
                .WithMany(p => p.LawyerSpecializations)
                .HasForeignKey(x => x.LawyerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Specialization>()
                .WithMany(p => p.LawyerSpecializations)
                .HasForeignKey(x => x.SpecializationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
