using System;
using System.Threading.Tasks;
using Sapienza.Lexus.usuUsuarios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.usuUsuarios;

public interface IusuUsuariosAppService :
    ICrudAppService<
        usuUsuariosDto,
        Guid,
        usuUsuariosGetListInput,
        CreateUpdateusuUsuariosDto,
        CreateUpdateusuUsuariosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetusuUsuariosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
