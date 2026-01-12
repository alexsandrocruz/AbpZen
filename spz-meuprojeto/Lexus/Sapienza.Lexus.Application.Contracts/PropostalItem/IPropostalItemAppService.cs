#nullable enable
using System;
using System.Threading.Tasks;
using Sapienza.Lexus.PropostalItem.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.PropostalItem;

public interface IPropostalItemAppService :
    ICrudAppService<
        PropostalItemDto,
        Guid,
        PropostalItemGetListInput,
        CreateUpdatePropostalItemDto,
        CreateUpdatePropostalItemDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetPropostalItemLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey? Id { get; set; }
    public string? DisplayName { get; set; }
}
