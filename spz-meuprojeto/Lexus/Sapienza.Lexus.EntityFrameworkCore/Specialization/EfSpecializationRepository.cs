using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Specialization;

public class EfSpecializationRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.Specialization.Specialization, Guid>, 
      ISpecializationRepository
{
    public EfSpecializationRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
