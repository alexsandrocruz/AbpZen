using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalBlockInstance;

public interface IProposalBlockInstanceRepository : IRepository<Sapienza.Dominus.ProposalBlockInstance.ProposalBlockInstance, Guid>
{
}
