using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.RAGDoc;

public class EfRAGDocRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.RAGDoc.RAGDoc, Guid>, 
      IRAGDocRepository
{
    public EfRAGDocRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
