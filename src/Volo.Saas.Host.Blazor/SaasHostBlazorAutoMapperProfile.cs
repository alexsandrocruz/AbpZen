using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Saas.Host.Blazor.Pages.Saas.Host;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Blazor;

public class SaasHostBlazorAutoMapperProfile : Profile
{
    public SaasHostBlazorAutoMapperProfile()
    {
        CreateMap<SaasTenantDto, SaasTenantUpdateDto>()
            .MapExtraProperties();

        CreateMap<EditionDto, EditionUpdateDto>()
            .MapExtraProperties();

        CreateMap<TenantConnectionStringsModel, SaasTenantConnectionStringsDto>()
            .MapExtraProperties()
            .ReverseMap()
            .Ignore(x => x.UseSharedDatabase)
            .Ignore(x => x.TenantName);

        CreateMap<TenantDatabaseConnectionStringsModel, SaasTenantDatabaseConnectionStringsDto>()
            .MapExtraProperties()
            .ReverseMap();
    }
}
