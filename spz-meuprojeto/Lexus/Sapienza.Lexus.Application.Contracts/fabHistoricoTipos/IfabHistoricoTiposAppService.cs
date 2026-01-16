using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabHistoricoTipos;

public interface IfabHistoricoTiposAppService :
    ICrudAppService<
        fabHistoricoTiposDto,
        Guid,
        fabHistoricoTiposGetListInput,
        CreateUpdatefabHistoricoTiposDto,
        CreateUpdatefabHistoricoTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabHistoricoTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
