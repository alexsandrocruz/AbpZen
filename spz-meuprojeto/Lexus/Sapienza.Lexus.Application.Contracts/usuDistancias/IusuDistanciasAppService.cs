using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuDistancias.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuDistancias;

public interface IusuDistanciasAppService :
    ICrudAppService<
        usuDistanciasDto,
        Guid,
        usuDistanciasGetListInput,
        CreateUpdateusuDistanciasDto,
        CreateUpdateusuDistanciasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuDistanciasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
