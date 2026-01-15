using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalItem;

public interface IProposalItemRepository : IRepository<Dominus.ProposalItem.ProposalItem, Guid>
{
}
