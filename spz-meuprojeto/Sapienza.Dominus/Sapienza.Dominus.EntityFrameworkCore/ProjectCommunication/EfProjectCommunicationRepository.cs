using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProjectCommunication;

public class EfProjectCommunicationRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ProjectCommunication.ProjectCommunication, Guid>, 
      IProjectCommunicationRepository
{
    public EfProjectCommunicationRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
