using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabPermissoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabPermissoes;

public interface IfabPermissoesAppService :
    ICrudAppService<
        fabPermissoesDto,
        Guid,
        fabPermissoesGetListInput,
        CreateUpdatefabPermissoesDto,
        CreateUpdatefabPermissoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabPermissoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
