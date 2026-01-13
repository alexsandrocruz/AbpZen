using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Lawyer;

public class EfLawyerRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.Lawyer.Lawyer, Guid>, 
      ILawyerRepository
{
    public EfLawyerRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
