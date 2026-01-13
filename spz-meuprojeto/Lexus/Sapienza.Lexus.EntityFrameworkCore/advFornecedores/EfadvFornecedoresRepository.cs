using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advFornecedores;

public class EfadvFornecedoresRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advFornecedores.advFornecedores, Guid>, 
      IadvFornecedoresRepository
{
    public EfadvFornecedoresRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
