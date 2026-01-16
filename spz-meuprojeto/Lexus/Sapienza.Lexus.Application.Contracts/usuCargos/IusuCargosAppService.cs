using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuCargos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuCargos;

public interface IusuCargosAppService :
    ICrudAppService<
        usuCargosDto,
        Guid,
        usuCargosGetListInput,
        CreateUpdateusuCargosDto,
        CreateUpdateusuCargosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuCargosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
