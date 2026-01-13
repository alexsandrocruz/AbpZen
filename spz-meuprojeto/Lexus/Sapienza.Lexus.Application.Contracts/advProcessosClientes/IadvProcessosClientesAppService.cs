using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessosClientes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessosClientes;

public interface IadvProcessosClientesAppService :
    ICrudAppService<
        advProcessosClientesDto,
        Guid,
        advProcessosClientesGetListInput,
        CreateUpdateadvProcessosClientesDto,
        CreateUpdateadvProcessosClientesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosClientesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
