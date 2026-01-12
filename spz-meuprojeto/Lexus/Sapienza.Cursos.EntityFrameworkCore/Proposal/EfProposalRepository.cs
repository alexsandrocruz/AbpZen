using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Proposal;

public class EfProposalRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Proposal.Proposal, Guid>, 
      IProposalRepository
{
    public EfProposalRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
