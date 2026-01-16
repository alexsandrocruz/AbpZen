using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesConvertidos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesConvertidos;

public interface IadvClientesConvertidosAppService :
    ICrudAppService<
        advClientesConvertidosDto,
        Guid,
        advClientesConvertidosGetListInput,
        CreateUpdateadvClientesConvertidosDto,
        CreateUpdateadvClientesConvertidosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesConvertidosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
