using System;
using System.Threading.Tasks;
using Sapienza.Lexus.RAGDoc.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.RAGDoc;

public interface IRAGDocAppService :
    ICrudAppService<
        RAGDocDto,
        Guid,
        RAGDocGetListInput,
        CreateUpdateRAGDocDto,
        CreateUpdateRAGDocDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetRAGDocLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
