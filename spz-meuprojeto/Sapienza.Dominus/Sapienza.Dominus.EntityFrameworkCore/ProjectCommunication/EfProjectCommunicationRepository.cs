using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProjectCommunication;

public class EfProjectCommunicationRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ProjectCommunication.ProjectCommunication, Guid>, 
      IProjectCommunicationRepository
{
    public EfProjectCommunicationRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
