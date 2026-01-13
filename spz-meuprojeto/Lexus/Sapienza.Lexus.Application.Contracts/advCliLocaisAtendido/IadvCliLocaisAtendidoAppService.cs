using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliLocaisAtendido.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliLocaisAtendido;

public interface IadvCliLocaisAtendidoAppService :
    ICrudAppService<
        advCliLocaisAtendidoDto,
        Guid,
        advCliLocaisAtendidoGetListInput,
        CreateUpdateadvCliLocaisAtendidoDto,
        CreateUpdateadvCliLocaisAtendidoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliLocaisAtendidoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
