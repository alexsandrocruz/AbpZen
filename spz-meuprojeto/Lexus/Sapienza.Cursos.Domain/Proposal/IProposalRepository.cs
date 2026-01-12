using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Proposal;

public interface IProposalRepository : IRepository<Sapienza.Cursos.Proposal.Proposal, Guid>
{
}
