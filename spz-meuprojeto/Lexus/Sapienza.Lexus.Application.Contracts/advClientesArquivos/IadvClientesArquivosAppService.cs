using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesArquivos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesArquivos;

public interface IadvClientesArquivosAppService :
    ICrudAppService<
        advClientesArquivosDto,
        Guid,
        advClientesArquivosGetListInput,
        CreateUpdateadvClientesArquivosDto,
        CreateUpdateadvClientesArquivosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesArquivosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
