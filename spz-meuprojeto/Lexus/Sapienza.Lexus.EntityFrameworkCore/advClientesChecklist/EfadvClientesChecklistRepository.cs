using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesChecklist;

public class EfadvClientesChecklistRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesChecklist.advClientesChecklist, Guid>, 
      IadvClientesChecklistRepository
{
    public EfadvClientesChecklistRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
