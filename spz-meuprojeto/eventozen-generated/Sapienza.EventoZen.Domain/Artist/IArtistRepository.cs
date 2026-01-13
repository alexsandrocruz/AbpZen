using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Artist;

public interface IArtistRepository : IRepository<Sapienza.EventoZen.Artist.Artist, Guid>
{
}
