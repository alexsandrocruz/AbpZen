using AutoMapper;
using Sapienza.Dominus.LeadStageHistory.Dtos;

namespace Sapienza.Dominus.LeadStageHistory;

public class LeadStageHistoryAutoMapperProfile : Profile
{
    public LeadStageHistoryAutoMapperProfile()
    {
        CreateMap<LeadStageHistory, LeadStageHistoryDto>()
            .ForMember(dest => dest.LeadDisplayName, opt => opt.MapFrom(src => src.Lead.Name));
        CreateMap<CreateUpdateLeadStageHistoryDto, LeadStageHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadStageHistoryDto, LeadStageHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
