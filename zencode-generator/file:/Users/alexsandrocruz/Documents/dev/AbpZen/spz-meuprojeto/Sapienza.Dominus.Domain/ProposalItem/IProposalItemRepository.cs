using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalItem;

public interface IProposalItemRepository : IRepository<Sapienza.Dominus.ProposalItem.ProposalItem, Guid>
{
}
