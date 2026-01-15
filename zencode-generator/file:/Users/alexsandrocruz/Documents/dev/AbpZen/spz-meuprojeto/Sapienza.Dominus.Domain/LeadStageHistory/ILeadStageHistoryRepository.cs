using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadStageHistory;

public interface ILeadStageHistoryRepository : IRepository<Sapienza.Dominus.LeadStageHistory.LeadStageHistory, Guid>
{
}
