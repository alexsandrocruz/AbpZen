using AutoMapper;
using Volo.Forms.Forms;

namespace Volo.Forms.Web;

public class FormsWebAutoMapperProfile : Profile
{
    public FormsWebAutoMapperProfile()
    {
        CreateMap<FormSettingsDto, UpdateFormSettingInputDto>();
    }
}
