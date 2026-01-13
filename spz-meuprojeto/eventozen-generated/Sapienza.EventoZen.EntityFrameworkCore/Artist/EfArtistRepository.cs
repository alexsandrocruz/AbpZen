using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.Artist;

public class EfArtistRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.Artist.Artist, Guid>, 
      IArtistRepository
{
    public EfArtistRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
