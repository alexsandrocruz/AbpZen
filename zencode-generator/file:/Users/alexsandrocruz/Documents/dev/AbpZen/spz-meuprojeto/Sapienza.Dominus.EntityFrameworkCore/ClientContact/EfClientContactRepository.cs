using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ClientContact;

public class EfClientContactRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ClientContact.ClientContact, Guid>, 
      IClientContactRepository
{
    public EfClientContactRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
