using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finAreas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finAreas;

public interface IfinAreasAppService :
    ICrudAppService<
        finAreasDto,
        Guid,
        finAreasGetListInput,
        CreateUpdatefinAreasDto,
        CreateUpdatefinAreasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinAreasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
