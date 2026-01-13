using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliTiposArquivos;

public class EfadvCliTiposArquivosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliTiposArquivos.advCliTiposArquivos, Guid>, 
      IadvCliTiposArquivosRepository
{
    public EfadvCliTiposArquivosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
