using System;
using Sapienza.EventoZen.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.EventoZen.ArtistSpecialty;

public class EfArtistSpecialtyRepository 
    : EfCoreRepository<EventoZenDbContext, Sapienza.EventoZen.ArtistSpecialty.ArtistSpecialty, Guid>, 
      IArtistSpecialtyRepository
{
    public EfArtistSpecialtyRepository(IDbContextProvider<EventoZenDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
