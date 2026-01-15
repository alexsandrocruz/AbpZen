using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.File;

public class EfFileRepository 
    : EfCoreRepository<DominusDbContext, Dominus.File.File, Guid>, 
      IFileRepository
{
    public EfFileRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
