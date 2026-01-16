using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreArquivosStatus;

public class EfadvPreArquivosStatusRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, Guid>, 
      IadvPreArquivosStatusRepository
{
    public EfadvPreArquivosStatusRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
