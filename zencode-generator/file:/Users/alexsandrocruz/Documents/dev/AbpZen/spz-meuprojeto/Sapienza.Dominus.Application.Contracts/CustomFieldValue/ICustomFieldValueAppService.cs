using System;
using System.Threading.Tasks;
using Sapienza.Dominus.CustomFieldValue.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.CustomFieldValue;

public interface ICustomFieldValueAppService :
    ICrudAppService<
        CustomFieldValueDto,
        Guid,
        CustomFieldValueGetListInput,
        CreateUpdateCustomFieldValueDto,
        CreateUpdateCustomFieldValueDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCustomFieldValueLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
