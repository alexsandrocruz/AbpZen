using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosHonorarios;

public class EfadvProcessosHonorariosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, Guid>, 
      IadvProcessosHonorariosRepository
{
    public EfadvProcessosHonorariosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
