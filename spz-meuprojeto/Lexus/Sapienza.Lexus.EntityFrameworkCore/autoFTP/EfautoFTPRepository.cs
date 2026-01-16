using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.autoFTP;

public class EfautoFTPRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.autoFTP.autoFTP, Guid>, 
      IautoFTPRepository
{
    public EfautoFTPRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
