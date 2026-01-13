using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.EventCommission;

public class EfEventCommissionRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.EventCommission.EventCommission, Guid>, 
      IEventCommissionRepository
{
    public EfEventCommissionRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
