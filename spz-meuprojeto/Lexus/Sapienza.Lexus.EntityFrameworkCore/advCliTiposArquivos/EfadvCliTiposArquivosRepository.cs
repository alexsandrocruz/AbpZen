using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliTiposArquivos;

public class EfadvCliTiposArquivosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, Guid>, 
      IadvCliTiposArquivosRepository
{
    public EfadvCliTiposArquivosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
