using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadAutomation;

public interface ILeadAutomationRepository : IRepository<Sapienza.Dominus.LeadAutomation.LeadAutomation, Guid>
{
}
