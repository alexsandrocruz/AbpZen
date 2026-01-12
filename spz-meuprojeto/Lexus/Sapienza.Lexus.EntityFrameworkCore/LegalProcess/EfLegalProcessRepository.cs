#nullable enable
using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.LegalProcess;

public class EfLegalProcessRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.LegalProcess.LegalProcess, Guid>, 
      ILegalProcessRepository
{
    public EfLegalProcessRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
