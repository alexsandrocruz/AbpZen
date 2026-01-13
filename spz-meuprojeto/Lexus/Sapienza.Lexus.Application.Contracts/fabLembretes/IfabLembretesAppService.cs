using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabLembretes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabLembretes;

public interface IfabLembretesAppService :
    ICrudAppService<
        fabLembretesDto,
        Guid,
        fabLembretesGetListInput,
        CreateUpdatefabLembretesDto,
        CreateUpdatefabLembretesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabLembretesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
