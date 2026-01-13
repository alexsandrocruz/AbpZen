using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuAreas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuAreas;

public interface IusuAreasAppService :
    ICrudAppService<
        usuAreasDto,
        Guid,
        usuAreasGetListInput,
        CreateUpdateusuAreasDto,
        CreateUpdateusuAreasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuAreasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
