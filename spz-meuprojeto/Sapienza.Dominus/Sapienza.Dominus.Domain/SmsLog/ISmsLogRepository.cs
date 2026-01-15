using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SmsLog;

public interface ISmsLogRepository : IRepository<Dominus.SmsLog.SmsLog, Guid>
{
}
