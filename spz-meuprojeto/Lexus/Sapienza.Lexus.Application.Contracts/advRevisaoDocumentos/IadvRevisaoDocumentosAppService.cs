using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advRevisaoDocumentos;

public interface IadvRevisaoDocumentosAppService :
    ICrudAppService<
        advRevisaoDocumentosDto,
        Guid,
        advRevisaoDocumentosGetListInput,
        CreateUpdateadvRevisaoDocumentosDto,
        CreateUpdateadvRevisaoDocumentosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvRevisaoDocumentosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
