using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuPermissoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuPermissoes;

public interface IusuPermissoesAppService :
    ICrudAppService<
        usuPermissoesDto,
        Guid,
        usuPermissoesGetListInput,
        CreateUpdateusuPermissoesDto,
        CreateUpdateusuPermissoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuPermissoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
