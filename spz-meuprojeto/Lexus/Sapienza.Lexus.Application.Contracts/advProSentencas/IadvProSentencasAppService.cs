using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProSentencas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProSentencas;

public interface IadvProSentencasAppService :
    ICrudAppService<
        advProSentencasDto,
        Guid,
        advProSentencasGetListInput,
        CreateUpdateadvProSentencasDto,
        CreateUpdateadvProSentencasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProSentencasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
