using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadScheduledMessage;

public interface ILeadScheduledMessageRepository : IRepository<Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid>
{
}
