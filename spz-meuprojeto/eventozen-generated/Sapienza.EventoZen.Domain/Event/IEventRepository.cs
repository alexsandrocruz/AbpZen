using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Event;

public interface IEventRepository : IRepository<Sapienza.EventoZen.Event.Event, Guid>
{
}
