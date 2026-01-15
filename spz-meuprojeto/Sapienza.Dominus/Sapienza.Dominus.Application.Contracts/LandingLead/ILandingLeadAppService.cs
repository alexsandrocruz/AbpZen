using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LandingLead.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LandingLead;

public interface ILandingLeadAppService :
    ICrudAppService<
        LandingLeadDto,
        Guid,
        LandingLeadGetListInput,
        CreateUpdateLandingLeadDto,
        CreateUpdateLandingLeadDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLandingLeadLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
