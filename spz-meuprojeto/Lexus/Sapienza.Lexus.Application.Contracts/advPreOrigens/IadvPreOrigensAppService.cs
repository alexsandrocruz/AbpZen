using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreOrigens.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreOrigens;

public interface IadvPreOrigensAppService :
    ICrudAppService<
        advPreOrigensDto,
        Guid,
        advPreOrigensGetListInput,
        CreateUpdateadvPreOrigensDto,
        CreateUpdateadvPreOrigensDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreOrigensLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
