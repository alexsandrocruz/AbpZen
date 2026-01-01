/**
 * AppService template for ABP
 */
export function getAppServiceTemplate(): string {
    return `using System;
using System.Linq;
using System.Threading.Tasks;
using {{ project.namespace }}.Permissions;
using {{ entity.namespace }}.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace {{ entity.namespace }};

public class {{ entity.name }}AppService : CrudAppService<
    {{ entity.name }},
    {{ dto.readTypeName }},
    {{ entity.primaryKey }},
    {{ entity.name }}GetListInput,
    {{ dto.createTypeName }},
    {{ dto.updateTypeName }}>,
    I{{ entity.name }}AppService
{
    protected override string? GetPolicyName { get; set; } = {{ project.name }}Permissions.{{ entity.name }}.Default;
    protected override string? GetListPolicyName { get; set; } = {{ project.name }}Permissions.{{ entity.name }}.Default;
    protected override string? CreatePolicyName { get; set; } = {{ project.name }}Permissions.{{ entity.name }}.Create;
    protected override string? UpdatePolicyName { get; set; } = {{ project.name }}Permissions.{{ entity.name }}.Update;
    protected override string? DeletePolicyName { get; set; } = {{ project.name }}Permissions.{{ entity.name }}.Delete;

    public {{ entity.name }}AppService(IRepository<{{ entity.name }}, {{ entity.primaryKey }}> repository) 
        : base(repository)
    {
    }

    protected override async Task<IQueryable<{{ entity.name }}>> CreateFilteredQueryAsync({{ entity.name }}GetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            {%- for field in entity.fields %}
            {%- if field.isFilterable %}
            {%- if field.type == 'string' %}
            .WhereIf(!input.{{ field.name }}.IsNullOrWhiteSpace(), x => x.{{ field.name }}.Contains(input.{{ field.name }}))
            {%- else %}
            .WhereIf(input.{{ field.name }} != null, x => x.{{ field.name }} == input.{{ field.name }})
            {%- endif %}
            {%- endif %}
            {%- endfor %}
            ;
    }
}
`;
}

/**
 * AppService interface template for ABP
 */
export function getAppServiceInterfaceTemplate(): string {
    return `using System;
using {{ entity.namespace }}.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace {{ entity.namespace }};

public interface I{{ entity.name }}AppService :
    ICrudAppService<
        {{ dto.readTypeName }},
        {{ entity.primaryKey }},
        {{ entity.name }}GetListInput,
        {{ dto.createTypeName }},
        {{ dto.updateTypeName }}>
{
}
`;
}
