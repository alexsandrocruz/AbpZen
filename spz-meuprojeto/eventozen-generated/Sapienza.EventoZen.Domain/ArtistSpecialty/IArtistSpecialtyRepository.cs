using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.ArtistSpecialty;

public interface IArtistSpecialtyRepository : IRepository<Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, Guid>
{
}
