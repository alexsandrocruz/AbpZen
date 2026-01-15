using AutoMapper;
using Sapienza.Dominus.LeadForm.Dtos;

namespace Sapienza.Dominus.LeadForm;

public class LeadFormAutoMapperProfile : Profile
{
    public LeadFormAutoMapperProfile()
    {
        CreateMap<LeadForm, LeadFormDto>()
            .ForMember(dest => dest.LeadWorkflowDisplayName, opt => opt.MapFrom(src => src.LeadWorkflow.Name));
        CreateMap<CreateUpdateLeadFormDto, LeadForm>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadFormDto, LeadForm>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
