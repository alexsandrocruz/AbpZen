using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliTiposArquivos;

public interface IadvCliTiposArquivosAppService :
    ICrudAppService<
        advCliTiposArquivosDto,
        Guid,
        advCliTiposArquivosGetListInput,
        CreateUpdateadvCliTiposArquivosDto,
        CreateUpdateadvCliTiposArquivosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliTiposArquivosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
