using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadFormSubmission.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadFormSubmission;

public interface ILeadFormSubmissionAppService :
    ICrudAppService<
        LeadFormSubmissionDto,
        Guid,
        LeadFormSubmissionGetListInput,
        CreateUpdateLeadFormSubmissionDto,
        CreateUpdateLeadFormSubmissionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadFormSubmissionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
