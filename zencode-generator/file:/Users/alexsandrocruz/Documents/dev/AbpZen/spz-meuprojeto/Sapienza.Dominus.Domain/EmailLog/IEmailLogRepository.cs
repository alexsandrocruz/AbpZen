using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.EmailLog;

public interface IEmailLogRepository : IRepository<Sapienza.Dominus.EmailLog.EmailLog, Guid>
{
}
