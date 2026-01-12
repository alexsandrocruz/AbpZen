#nullable enable
using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Proposal;

public interface IProposalRepository : IRepository<Sapienza.Lexus.Proposal.Proposal, Guid>
{
}
