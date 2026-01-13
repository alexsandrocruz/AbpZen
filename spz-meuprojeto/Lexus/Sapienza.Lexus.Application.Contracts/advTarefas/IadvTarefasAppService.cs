using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advTarefas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advTarefas;

public interface IadvTarefasAppService :
    ICrudAppService<
        advTarefasDto,
        Guid,
        advTarefasGetListInput,
        CreateUpdateadvTarefasDto,
        CreateUpdateadvTarefasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvTarefasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
