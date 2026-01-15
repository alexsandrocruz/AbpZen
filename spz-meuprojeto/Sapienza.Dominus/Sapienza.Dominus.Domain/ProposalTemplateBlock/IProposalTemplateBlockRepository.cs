using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalTemplateBlock;

public interface IProposalTemplateBlockRepository : IRepository<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, Guid>
{
}
