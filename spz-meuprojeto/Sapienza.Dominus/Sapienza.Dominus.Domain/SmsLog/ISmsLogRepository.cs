using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SmsLog;

public interface ISmsLogRepository : IRepository<Sapienza.Dominus.SmsLog.SmsLog, Guid>
{
}
