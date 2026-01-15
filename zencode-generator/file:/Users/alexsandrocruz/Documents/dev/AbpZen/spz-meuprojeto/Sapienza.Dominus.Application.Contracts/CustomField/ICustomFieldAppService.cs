using System;
using System.Threading.Tasks;
using Sapienza.Dominus.CustomField.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.CustomField;

public interface ICustomFieldAppService :
    ICrudAppService<
        CustomFieldDto,
        Guid,
        CustomFieldGetListInput,
        CreateUpdateCustomFieldDto,
        CreateUpdateCustomFieldDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCustomFieldLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
