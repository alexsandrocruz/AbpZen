using System;
using System.Threading.Tasks;
using Sapienza.Dominus.File.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.File;

public interface IFileAppService :
    ICrudAppService<
        FileDto,
        Guid,
        FileGetListInput,
        CreateUpdateFileDto,
        CreateUpdateFileDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetFileLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
