using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advRevisaoDocumentos;

public class EfadvRevisaoDocumentosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, Guid>, 
      IadvRevisaoDocumentosRepository
{
    public EfadvRevisaoDocumentosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
