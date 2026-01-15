using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Proposal;

public interface IProposalRepository : IRepository<Sapienza.Dominus.Proposal.Proposal, Guid>
{
}
