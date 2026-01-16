using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advRevisaoDocumentos;

public class EfadvRevisaoDocumentosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, Guid>, 
      IadvRevisaoDocumentosRepository
{
    public EfadvRevisaoDocumentosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
