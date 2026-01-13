using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreArquivosStatus;

public class EfadvPreArquivosStatusRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, Guid>, 
      IadvPreArquivosStatusRepository
{
    public EfadvPreArquivosStatusRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
