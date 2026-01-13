using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.EventCommission;

public interface IEventCommissionRepository : IRepository<Sapienza.EventoZen.EventCommission.EventCommission, Guid>
{
}
