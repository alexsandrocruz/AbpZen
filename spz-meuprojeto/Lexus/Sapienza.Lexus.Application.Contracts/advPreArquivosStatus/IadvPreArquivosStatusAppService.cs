using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreArquivosStatus;

public interface IadvPreArquivosStatusAppService :
    ICrudAppService<
        advPreArquivosStatusDto,
        Guid,
        advPreArquivosStatusGetListInput,
        CreateUpdateadvPreArquivosStatusDto,
        CreateUpdateadvPreArquivosStatusDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreArquivosStatusLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
