using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadScheduledMessage;

public interface ILeadScheduledMessageRepository : IRepository<Sapienza.Dominus.LeadScheduledMessage.LeadScheduledMessage, Guid>
{
}
