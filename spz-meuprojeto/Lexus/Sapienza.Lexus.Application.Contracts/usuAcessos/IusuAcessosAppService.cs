using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuAcessos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuAcessos;

public interface IusuAcessosAppService :
    ICrudAppService<
        usuAcessosDto,
        Guid,
        usuAcessosGetListInput,
        CreateUpdateusuAcessosDto,
        CreateUpdateusuAcessosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuAcessosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
