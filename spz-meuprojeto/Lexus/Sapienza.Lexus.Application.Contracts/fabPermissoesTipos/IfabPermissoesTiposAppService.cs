using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabPermissoesTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabPermissoesTipos;

public interface IfabPermissoesTiposAppService :
    ICrudAppService<
        fabPermissoesTiposDto,
        Guid,
        fabPermissoesTiposGetListInput,
        CreateUpdatefabPermissoesTiposDto,
        CreateUpdatefabPermissoesTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabPermissoesTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
