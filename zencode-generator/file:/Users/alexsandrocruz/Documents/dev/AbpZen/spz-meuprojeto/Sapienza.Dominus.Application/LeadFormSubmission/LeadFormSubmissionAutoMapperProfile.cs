using AutoMapper;
using Sapienza.Dominus.LeadFormSubmission.Dtos;

namespace Sapienza.Dominus.LeadFormSubmission;

public class LeadFormSubmissionAutoMapperProfile : Profile
{
    public LeadFormSubmissionAutoMapperProfile()
    {
        CreateMap<LeadFormSubmission, LeadFormSubmissionDto>()
            .ForMember(dest => dest.LeadFormDisplayName, opt => opt.MapFrom(src => src.LeadForm.Name));
        CreateMap<CreateUpdateLeadFormSubmissionDto, LeadFormSubmission>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadFormSubmissionDto, LeadFormSubmission>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
