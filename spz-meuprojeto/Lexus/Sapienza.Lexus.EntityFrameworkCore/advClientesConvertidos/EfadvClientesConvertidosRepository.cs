using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesConvertidos;

public class EfadvClientesConvertidosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesConvertidos.advClientesConvertidos, Guid>, 
      IadvClientesConvertidosRepository
{
    public EfadvClientesConvertidosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
