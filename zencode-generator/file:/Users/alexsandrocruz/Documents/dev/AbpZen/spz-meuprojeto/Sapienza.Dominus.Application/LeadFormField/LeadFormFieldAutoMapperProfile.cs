using AutoMapper;
using Sapienza.Dominus.LeadFormField.Dtos;

namespace Sapienza.Dominus.LeadFormField;

public class LeadFormFieldAutoMapperProfile : Profile
{
    public LeadFormFieldAutoMapperProfile()
    {
        CreateMap<LeadFormField, LeadFormFieldDto>()
            .ForMember(dest => dest.LeadFormDisplayName, opt => opt.MapFrom(src => src.LeadForm.Name));
        CreateMap<CreateUpdateLeadFormFieldDto, LeadFormField>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadFormFieldDto, LeadFormField>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
