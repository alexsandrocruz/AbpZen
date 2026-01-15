using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Proposal;

public interface IProposalRepository : IRepository<Dominus.Proposal.Proposal, Guid>
{
}
