using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WhatsappLog;

public interface IWhatsappLogRepository : IRepository<Dominus.WhatsappLog.WhatsappLog, Guid>
{
}
