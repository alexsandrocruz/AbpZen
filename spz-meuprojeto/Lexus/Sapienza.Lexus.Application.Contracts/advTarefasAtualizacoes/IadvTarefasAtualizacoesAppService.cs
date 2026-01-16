using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advTarefasAtualizacoes;

public interface IadvTarefasAtualizacoesAppService :
    ICrudAppService<
        advTarefasAtualizacoesDto,
        Guid,
        advTarefasAtualizacoesGetListInput,
        CreateUpdateadvTarefasAtualizacoesDto,
        CreateUpdateadvTarefasAtualizacoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvTarefasAtualizacoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
