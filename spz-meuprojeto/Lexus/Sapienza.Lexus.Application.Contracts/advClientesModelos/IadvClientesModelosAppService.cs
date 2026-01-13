using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesModelos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesModelos;

public interface IadvClientesModelosAppService :
    ICrudAppService<
        advClientesModelosDto,
        Guid,
        advClientesModelosGetListInput,
        CreateUpdateadvClientesModelosDto,
        CreateUpdateadvClientesModelosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesModelosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
