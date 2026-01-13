using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Location;

public interface ILocationRepository : IRepository<Sapienza.EventoZen.Location.Location, Guid>
{
}
