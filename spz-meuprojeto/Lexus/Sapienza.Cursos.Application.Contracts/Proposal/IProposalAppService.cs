using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Proposal.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Proposal;

public interface IProposalAppService :
    ICrudAppService<
        ProposalDto,
        Guid,
        ProposalGetListInput,
        CreateUpdateProposalDto,
        CreateUpdateProposalDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProposalLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
