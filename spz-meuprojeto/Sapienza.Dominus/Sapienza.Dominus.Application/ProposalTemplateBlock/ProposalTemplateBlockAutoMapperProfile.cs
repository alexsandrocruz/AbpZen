using AutoMapper;
using Sapienza.Dominus.ProposalTemplateBlock.Dtos;

namespace Sapienza.Dominus.ProposalTemplateBlock;

public class ProposalTemplateBlockAutoMapperProfile : Profile
{
    public ProposalTemplateBlockAutoMapperProfile()
    {
        CreateMap<ProposalTemplateBlock, ProposalTemplateBlockDto>();
        CreateMap<CreateUpdateProposalTemplateBlockDto, ProposalTemplateBlock>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateProposalTemplateBlockDto, ProposalTemplateBlock>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
