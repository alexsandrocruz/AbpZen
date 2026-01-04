using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.LeadMessage;

public interface ILeadMessageRepository : IRepository<LeptonXDemoApp.LeadMessage.LeadMessage, Guid>
{
}
