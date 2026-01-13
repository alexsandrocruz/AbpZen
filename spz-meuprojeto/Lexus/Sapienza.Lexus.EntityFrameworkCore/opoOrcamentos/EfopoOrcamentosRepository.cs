using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoOrcamentos;

public class EfopoOrcamentosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.opoOrcamentos.opoOrcamentos, Guid>, 
      IopoOrcamentosRepository
{
    public EfopoOrcamentosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
