using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesAtualizacoes;

public interface IadvClientesAtualizacoesAppService :
    ICrudAppService<
        advClientesAtualizacoesDto,
        Guid,
        advClientesAtualizacoesGetListInput,
        CreateUpdateadvClientesAtualizacoesDto,
        CreateUpdateadvClientesAtualizacoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesAtualizacoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
