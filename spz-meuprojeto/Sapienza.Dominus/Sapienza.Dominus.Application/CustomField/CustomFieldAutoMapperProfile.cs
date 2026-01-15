using AutoMapper;
using Sapienza.Dominus.CustomField.Dtos;

namespace Sapienza.Dominus.CustomField;

public class CustomFieldAutoMapperProfile : Profile
{
    public CustomFieldAutoMapperProfile()
    {
        CreateMap<CustomField, CustomFieldDto>();
        CreateMap<CreateUpdateCustomFieldDto, CustomField>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateCustomFieldDto, CustomField>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
