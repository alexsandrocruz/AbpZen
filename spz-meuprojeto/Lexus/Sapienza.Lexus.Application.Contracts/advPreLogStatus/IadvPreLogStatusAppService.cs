using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreLogStatus.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreLogStatus;

public interface IadvPreLogStatusAppService :
    ICrudAppService<
        advPreLogStatusDto,
        Guid,
        advPreLogStatusGetListInput,
        CreateUpdateadvPreLogStatusDto,
        CreateUpdateadvPreLogStatusDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreLogStatusLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
