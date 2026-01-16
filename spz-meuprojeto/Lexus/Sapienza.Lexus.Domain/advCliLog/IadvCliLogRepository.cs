using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliLog;

public interface IadvCliLogRepository : IRepository<Sapienza.Lexus.advCliLog.advCliLog, Guid>
{
}
