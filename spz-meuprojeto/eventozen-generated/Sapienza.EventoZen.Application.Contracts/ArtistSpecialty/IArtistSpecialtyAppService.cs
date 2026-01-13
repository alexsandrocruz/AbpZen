using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.ArtistSpecialty.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.ArtistSpecialty;

public interface IArtistSpecialtyAppService :
    ICrudAppService<
        ArtistSpecialtyDto,
        Guid,
        ArtistSpecialtyGetListInput,
        CreateUpdateArtistSpecialtyDto,
        CreateUpdateArtistSpecialtyDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetArtistSpecialtyLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
