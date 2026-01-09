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
            // <GEN-MAPPINGS>
        }
    }
}
