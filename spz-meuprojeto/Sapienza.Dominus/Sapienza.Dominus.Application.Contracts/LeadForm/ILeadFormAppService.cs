using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadForm.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadForm;

public interface ILeadFormAppService :
    ICrudAppService<
        LeadFormDto,
        Guid,
        LeadFormGetListInput,
        CreateUpdateLeadFormDto,
        CreateUpdateLeadFormDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadFormLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
