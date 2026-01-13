using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabPaises.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabPaises;

public interface IfabPaisesAppService :
    ICrudAppService<
        fabPaisesDto,
        Guid,
        fabPaisesGetListInput,
        CreateUpdatefabPaisesDto,
        CreateUpdatefabPaisesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabPaisesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
