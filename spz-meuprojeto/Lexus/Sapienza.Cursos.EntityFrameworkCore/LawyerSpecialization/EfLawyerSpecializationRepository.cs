using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.LawyerSpecialization;

public class EfLawyerSpecializationRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, Guid>, 
      ILawyerSpecializationRepository
{
    public EfLawyerSpecializationRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
