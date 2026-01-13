using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Availability;

public interface IAvailabilityRepository : IRepository<Sapienza.EventoZen.Availability.Availability, Guid>
{
}
