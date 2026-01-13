using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessosDadosHerdeiros.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

public interface IadvProcessosDadosHerdeirosAppService :
    ICrudAppService<
        advProcessosDadosHerdeirosDto,
        Guid,
        advProcessosDadosHerdeirosGetListInput,
        CreateUpdateadvProcessosDadosHerdeirosDto,
        CreateUpdateadvProcessosDadosHerdeirosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosDadosHerdeirosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
