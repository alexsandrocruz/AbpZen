using AutoMapper;
using LeptonXDemoApp.Lead.Dtos;

namespace LeptonXDemoApp.Lead;

public class LeadAutoMapperProfile : Profile
{
    public LeadAutoMapperProfile()
    {
        CreateMap<Lead, LeadDto>();
        CreateMap<CreateUpdateLeadDto, Lead>();
        CreateMap<CreateUpdateLeadDto, Lead>();
    }
}
