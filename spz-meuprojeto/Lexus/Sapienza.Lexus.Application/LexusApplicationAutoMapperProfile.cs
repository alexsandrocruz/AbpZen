using AutoMapper;

namespace Sapienza.Lexus
{
    public class LexusApplicationAutoMapperProfile : Profile
    {
        public LexusApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Sapienza.Lexus.Lawyer.Lawyer, Sapienza.Lexus.Lawyer.Dtos.LawyerDto>();
            CreateMap<Sapienza.Lexus.Lawyer.Dtos.CreateUpdateLawyerDto, Sapienza.Lexus.Lawyer.Lawyer>();

            CreateMap<Sapienza.Lexus.Case.Case, Sapienza.Lexus.Case.Dtos.CaseDto>();
            CreateMap<Sapienza.Lexus.Case.Dtos.CreateUpdateCaseDto, Sapienza.Lexus.Case.Case>();

            CreateMap<Sapienza.Lexus.Client.Client, Sapienza.Lexus.Client.Dtos.ClientDto>();
            CreateMap<Sapienza.Lexus.Client.Dtos.CreateUpdateClientDto, Sapienza.Lexus.Client.Client>();

            CreateMap<Sapienza.Lexus.Specialization.Specialization, Sapienza.Lexus.Specialization.Dtos.SpecializationDto>();
            CreateMap<Sapienza.Lexus.Specialization.Dtos.CreateUpdateSpecializationDto, Sapienza.Lexus.Specialization.Specialization>();

            CreateMap<Sapienza.Lexus.LegalProcess.LegalProcess, Sapienza.Lexus.LegalProcess.Dtos.LegalProcessDto>();
            CreateMap<Sapienza.Lexus.LegalProcess.Dtos.CreateUpdateLegalProcessDto, Sapienza.Lexus.LegalProcess.LegalProcess>();

            CreateMap<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization, Sapienza.Lexus.LawyerSpecialization.Dtos.LawyerSpecializationDto>();
            CreateMap<Sapienza.Lexus.LawyerSpecialization.Dtos.CreateUpdateLawyerSpecializationDto, Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();
                        CreateMap<Sapienza.Lexus.Proposal.Proposal, Sapienza.Lexus.Proposal.Dtos.ProposalDto>();
            CreateMap<Sapienza.Lexus.Proposal.Dtos.CreateUpdateProposalDto, Sapienza.Lexus.Proposal.Proposal>();
                  CreateMap<Sapienza.Lexus.PropostalItem.PropostalItem, Sapienza.Lexus.PropostalItem.Dtos.PropostalItemDto>();
            CreateMap<Sapienza.Lexus.PropostalItem.Dtos.CreateUpdatePropostalItemDto, Sapienza.Lexus.PropostalItem.PropostalItem>();
      // <<GEN-MAPPINGS>>
        }
    }
}
