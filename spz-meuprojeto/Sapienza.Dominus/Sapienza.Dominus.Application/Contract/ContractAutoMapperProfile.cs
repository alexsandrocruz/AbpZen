using AutoMapper;
using Sapienza.Dominus.Contract.Dtos;

namespace Sapienza.Dominus.Contract;

public class ContractAutoMapperProfile : Profile
{
    public ContractAutoMapperProfile()
    {
        CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateContractDto, Contract>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateContractDto, Contract>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
