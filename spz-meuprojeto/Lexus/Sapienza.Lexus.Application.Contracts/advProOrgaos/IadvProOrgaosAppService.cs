using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProOrgaos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProOrgaos;

public interface IadvProOrgaosAppService :
    ICrudAppService<
        advProOrgaosDto,
        Guid,
        advProOrgaosGetListInput,
        CreateUpdateadvProOrgaosDto,
        CreateUpdateadvProOrgaosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProOrgaosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
