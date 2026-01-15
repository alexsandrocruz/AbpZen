using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProposalBlockInstance.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProposalBlockInstance;

public interface IProposalBlockInstanceAppService :
    ICrudAppService<
        ProposalBlockInstanceDto,
        Guid,
        ProposalBlockInstanceGetListInput,
        CreateUpdateProposalBlockInstanceDto,
        CreateUpdateProposalBlockInstanceDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProposalBlockInstanceLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
