using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Proposal;

public class EfProposalRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.Proposal.Proposal, Guid>, 
      IProposalRepository
{
    public EfProposalRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
