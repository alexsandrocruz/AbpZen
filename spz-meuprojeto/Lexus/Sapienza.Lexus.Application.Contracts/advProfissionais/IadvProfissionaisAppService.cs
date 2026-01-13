using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProfissionais.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProfissionais;

public interface IadvProfissionaisAppService :
    ICrudAppService<
        advProfissionaisDto,
        Guid,
        advProfissionaisGetListInput,
        CreateUpdateadvProfissionaisDto,
        CreateUpdateadvProfissionaisDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
