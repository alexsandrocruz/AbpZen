using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.PropostalItem;

public interface IPropostalItemRepository : IRepository<Sapienza.Lexus.PropostalItem.PropostalItem, Guid>
{
}
