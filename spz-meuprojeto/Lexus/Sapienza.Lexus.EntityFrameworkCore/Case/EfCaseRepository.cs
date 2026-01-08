using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Case;

public class EfCaseRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.Case.Case, Guid>, 
      ICaseRepository
{
    public EfCaseRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
