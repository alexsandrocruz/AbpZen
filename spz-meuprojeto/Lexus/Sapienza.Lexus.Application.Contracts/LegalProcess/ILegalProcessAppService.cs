#nullable enable
using System;
using System.Threading.Tasks;
using Sapienza.Lexus.LegalProcess.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.LegalProcess;

public interface ILegalProcessAppService :
    ICrudAppService<
        LegalProcessDto,
        Guid,
        LegalProcessGetListInput,
        CreateUpdateLegalProcessDto,
        CreateUpdateLegalProcessDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLegalProcessLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey? Id { get; set; }
    public string? DisplayName { get; set; }
}
