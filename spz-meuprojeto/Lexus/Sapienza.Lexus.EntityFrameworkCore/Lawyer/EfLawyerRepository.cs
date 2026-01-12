#nullable enable
using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Lawyer;

public class EfLawyerRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.Lawyer.Lawyer, Guid>, 
      ILawyerRepository
{
    public EfLawyerRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
