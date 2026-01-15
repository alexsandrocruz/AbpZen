using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProposalTemplateBlock.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProposalTemplateBlock;

public interface IProposalTemplateBlockAppService :
    ICrudAppService<
        ProposalTemplateBlockDto,
        Guid,
        ProposalTemplateBlockGetListInput,
        CreateUpdateProposalTemplateBlockDto,
        CreateUpdateProposalTemplateBlockDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProposalTemplateBlockLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
