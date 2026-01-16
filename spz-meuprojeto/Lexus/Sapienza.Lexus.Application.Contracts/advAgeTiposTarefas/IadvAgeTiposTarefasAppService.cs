using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advAgeTiposTarefas;

public interface IadvAgeTiposTarefasAppService :
    ICrudAppService<
        advAgeTiposTarefasDto,
        Guid,
        advAgeTiposTarefasGetListInput,
        CreateUpdateadvAgeTiposTarefasDto,
        CreateUpdateadvAgeTiposTarefasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvAgeTiposTarefasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
