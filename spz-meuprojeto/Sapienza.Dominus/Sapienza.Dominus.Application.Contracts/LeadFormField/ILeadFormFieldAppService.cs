using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadFormField.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadFormField;

public interface ILeadFormFieldAppService :
    ICrudAppService<
        LeadFormFieldDto,
        Guid,
        LeadFormFieldGetListInput,
        CreateUpdateLeadFormFieldDto,
        CreateUpdateLeadFormFieldDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadFormFieldLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
