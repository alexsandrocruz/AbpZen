using System;
using System.Threading.Tasks;
using Sapienza.Cursos.NewEntity1.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.NewEntity1;

public interface INewEntity1AppService :
    ICrudAppService<
        NewEntity1Dto,
        Guid,
        NewEntity1GetListInput,
        CreateUpdateNewEntity1Dto,
        CreateUpdateNewEntity1Dto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetNewEntity1LookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
