using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabRegioes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabRegioes;

public interface IfabRegioesAppService :
    ICrudAppService<
        fabRegioesDto,
        Guid,
        fabRegioesGetListInput,
        CreateUpdatefabRegioesDto,
        CreateUpdatefabRegioesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabRegioesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
