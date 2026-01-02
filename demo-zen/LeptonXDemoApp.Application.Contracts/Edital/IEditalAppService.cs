using System;
using LeptonXDemoApp.Edital.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Edital;

public interface IEditalAppService :
    ICrudAppService<
        EditalDto,
        Guid,
        EditalGetListInput,
        CreateUpdateEditalDto,
        CreateUpdateEditalDto>
{
}
