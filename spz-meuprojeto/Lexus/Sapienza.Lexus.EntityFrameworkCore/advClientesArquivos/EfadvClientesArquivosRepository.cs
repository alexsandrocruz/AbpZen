using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesArquivos;

public class EfadvClientesArquivosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advClientesArquivos.advClientesArquivos, Guid>, 
      IadvClientesArquivosRepository
{
    public EfadvClientesArquivosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
