using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.LawyerSpecialization;

public class EfLawyerSpecializationRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization, Guid>, 
      ILawyerSpecializationRepository
{
    public EfLawyerSpecializationRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
