using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Host.Pages.Saas.Host.Tenants;

namespace Volo.Saas.Host;

public class SaasHostWebAutoMapperProfile : Profile
{
    public SaasHostWebAutoMapperProfile()
    {
        CreateMap<SaasTenantDto, EditModalModel.TenantInfoModel>()
            .MapExtraProperties();

        CreateMap<CreateModalModel.TenantConnectionStringsModel, SaasTenantConnectionStringsDto>()
            .MapExtraProperties();
        CreateMap<CreateModalModel.TenantDatabaseConnectionStringsModel, SaasTenantDatabaseConnectionStringsDto>()
            .MapExtraProperties();
        CreateMap<CreateModalModel.TenantInfoModel, SaasTenantCreateDto>()
            .Ignore(i => i.EditionEndDateUtc)
            .MapExtraProperties();

        CreateMap<EditModalModel.TenantInfoModel, SaasTenantUpdateDto>()
            .Ignore(i => i.EditionEndDateUtc)
            .MapExtraProperties();

        CreateMap<ConnectionStringsModal.TenantConnectionStringsModel, SaasTenantConnectionStringsDto>()
            .MapExtraProperties()
            .ReverseMap()
            .Ignore(x => x.UseSharedDatabase)
            .Ignore(x => x.UseSharedDatabase)
            .Ignore(x => x.UseSpecificDatabase);

        CreateMap<ConnectionStringsModal.TenantDatabaseConnectionStringsModel, SaasTenantDatabaseConnectionStringsDto>()
            .MapExtraProperties()
            .ReverseMap();

        CreateMap<EditionDto, Pages.Saas.Host.Editions.EditModalModel.EditionInfoModel>()
            .MapExtraProperties();

        CreateMap<Pages.Saas.Host.Editions.CreateModalModel.EditionInfoModel, EditionCreateDto>()
            .MapExtraProperties();

        CreateMap<Pages.Saas.Host.Editions.EditModalModel.EditionInfoModel, EditionUpdateDto>()
            .MapExtraProperties();
    }
}
