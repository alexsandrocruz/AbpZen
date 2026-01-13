using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.LegalProcess;

public class EfLegalProcessRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.LegalProcess.LegalProcess, Guid>, 
      ILegalProcessRepository
{
    public EfLegalProcessRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
