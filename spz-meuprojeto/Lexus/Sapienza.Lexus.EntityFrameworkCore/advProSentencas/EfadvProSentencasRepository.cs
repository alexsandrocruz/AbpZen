using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProSentencas;

public class EfadvProSentencasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProSentencas.advProSentencas, Guid>, 
      IadvProSentencasRepository
{
    public EfadvProSentencasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
