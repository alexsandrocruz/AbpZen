using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProposalTemplateBlock;

public class EfProposalTemplateBlockRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ProposalTemplateBlock.ProposalTemplateBlock, Guid>, 
      IProposalTemplateBlockRepository
{
    public EfProposalTemplateBlockRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
