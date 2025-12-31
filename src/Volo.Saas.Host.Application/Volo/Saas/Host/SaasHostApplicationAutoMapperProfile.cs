using System.Collections.Generic;
using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Saas.Editions;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Tenants;

namespace Volo.Saas.Host;

public class SaasHostApplicationAutoMapperProfile : Profile
{
    public SaasHostApplicationAutoMapperProfile()
    {
        CreateMap<Tenant, SaasTenantDto>()
            .MapExtraProperties()
            .Ignore(t => t.EditionName)
            .ForMember(
                t => t.HasDefaultConnectionString,
                opt =>
                {
                    opt.MapFrom(t =>
                        !t.ConnectionStrings.IsNullOrEmpty()
                    );
                }
            );

        CreateMap<Edition, EditionDto>()
            .MapExtraProperties()
            .Ignore(x => x.PlanName)
            .Ignore(x => x.TenantCount);

        CreateMap<Edition, EditionLookupDto>()
            .MapExtraProperties();
    }
}
