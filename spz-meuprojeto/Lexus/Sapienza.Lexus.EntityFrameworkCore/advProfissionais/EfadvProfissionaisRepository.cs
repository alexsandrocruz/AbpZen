using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProfissionais;

public class EfadvProfissionaisRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProfissionais.advProfissionais, Guid>, 
      IadvProfissionaisRepository
{
    public EfadvProfissionaisRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
