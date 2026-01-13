using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.Artist.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.Artist;

public interface IArtistAppService :
    ICrudAppService<
        ArtistDto,
        Guid,
        ArtistGetListInput,
        CreateUpdateArtistDto,
        CreateUpdateArtistDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetArtistLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
