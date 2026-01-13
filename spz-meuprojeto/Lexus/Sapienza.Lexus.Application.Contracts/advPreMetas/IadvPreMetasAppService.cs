using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreMetas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreMetas;

public interface IadvPreMetasAppService :
    ICrudAppService<
        advPreMetasDto,
        Guid,
        advPreMetasGetListInput,
        CreateUpdateadvPreMetasDto,
        CreateUpdateadvPreMetasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreMetasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
