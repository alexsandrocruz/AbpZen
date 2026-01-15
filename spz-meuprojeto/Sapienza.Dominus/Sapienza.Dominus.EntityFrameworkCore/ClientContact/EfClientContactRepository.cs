using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ClientContact;

public class EfClientContactRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ClientContact.ClientContact, Guid>, 
      IClientContactRepository
{
    public EfClientContactRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
