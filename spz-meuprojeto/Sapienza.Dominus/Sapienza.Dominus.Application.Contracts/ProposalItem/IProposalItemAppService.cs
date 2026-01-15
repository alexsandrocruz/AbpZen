using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProposalItem.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProposalItem;

public interface IProposalItemAppService :
    ICrudAppService<
        ProposalItemDto,
        Guid,
        ProposalItemGetListInput,
        CreateUpdateProposalItemDto,
        CreateUpdateProposalItemDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProposalItemLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
