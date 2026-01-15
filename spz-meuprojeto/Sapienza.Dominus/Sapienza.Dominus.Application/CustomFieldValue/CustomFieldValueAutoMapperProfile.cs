using AutoMapper;
using Sapienza.Dominus.CustomFieldValue.Dtos;

namespace Sapienza.Dominus.CustomFieldValue;

public class CustomFieldValueAutoMapperProfile : Profile
{
    public CustomFieldValueAutoMapperProfile()
    {
        CreateMap<CustomFieldValue, CustomFieldValueDto>()
            .ForMember(dest => dest.CustomFieldDisplayName, opt => opt.MapFrom(src => src.CustomField.EntityType));
        CreateMap<CreateUpdateCustomFieldValueDto, CustomFieldValue>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateCustomFieldValueDto, CustomFieldValue>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
