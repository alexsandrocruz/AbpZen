using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.autoFTP;

public class EfautoFTPRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.autoFTP.autoFTP, Guid>, 
      IautoFTPRepository
{
    public EfautoFTPRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
