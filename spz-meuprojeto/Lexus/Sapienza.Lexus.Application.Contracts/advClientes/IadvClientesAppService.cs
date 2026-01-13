using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientes;

public interface IadvClientesAppService :
    ICrudAppService<
        advClientesDto,
        Guid,
        advClientesGetListInput,
        CreateUpdateadvClientesDto,
        CreateUpdateadvClientesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
