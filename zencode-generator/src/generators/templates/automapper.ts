/**
 * AutoMapper profile template for Application layer
 */
export function getAutoMapperProfileTemplate(): string {
    return `using AutoMapper;
using {{ project.namespace }}.{{ entity.name }}.Dtos;

namespace {{ project.namespace }}.{{ entity.name }};

public class {{ entity.name }}AutoMapperProfile : Profile
{
    public {{ entity.name }}AutoMapperProfile()
    {
        CreateMap<{{ entity.name }}, {{ dto.readTypeName }}>();
        CreateMap<{{ dto.createTypeName }}, {{ entity.name }}>();
        CreateMap<{{ dto.updateTypeName }}, {{ entity.name }}>();
    }
}
`;
}

/**
 * Application AutoMapper profile additions (snippet to add to existing profile)
 */
export function getAutoMapperProfileSnippet(): string {
    return `        CreateMap<{{ entity.name }}, {{ dto.readTypeName }}>();
        CreateMap<{{ dto.createTypeName }}, {{ entity.name }}>();
`;
}
