using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finProcuracoesRPV;

public class EffinProcuracoesRPVRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finProcuracoesRPV.finProcuracoesRPV, Guid>, 
      IfinProcuracoesRPVRepository
{
    public EffinProcuracoesRPVRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
