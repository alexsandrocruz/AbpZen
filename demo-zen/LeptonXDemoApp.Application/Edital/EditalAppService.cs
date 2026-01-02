using System;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Edital.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Edital;

public class EditalAppService : CrudAppService<
    Edital,
    EditalDto,
    Guid,
    EditalGetListInput,
    CreateUpdateEditalDto,
    CreateUpdateEditalDto>,
    IEditalAppService
{
    protected override string? GetPolicyName { get; set; } = LeptonXDemoAppPermissions.Edital.Default;
    protected override string? GetListPolicyName { get; set; } = LeptonXDemoAppPermissions.Edital.Default;
    protected override string? CreatePolicyName { get; set; } = LeptonXDemoAppPermissions.Edital.Create;
    protected override string? UpdatePolicyName { get; set; } = LeptonXDemoAppPermissions.Edital.Update;
    protected override string? DeletePolicyName { get; set; } = LeptonXDemoAppPermissions.Edital.Delete;

    public EditalAppService(IRepository<Edital, Guid> repository) 
        : base(repository)
    {
    }

    protected override async Task<IQueryable<Edital>> CreateFilteredQueryAsync(EditalGetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Objeto.IsNullOrWhiteSpace(), x => x.Objeto.Contains(input.Objeto))
            .WhereIf(input.Data != null, x => x.Data == input.Data)
            .WhereIf(input.Valor != null, x => x.Valor == input.Valor)
            ;
    }
}
