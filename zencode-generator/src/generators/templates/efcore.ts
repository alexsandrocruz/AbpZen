/**
 * DbContext model configuration extensions template
 */
export function getDbContextExtensionsTemplate(): string {
    return `using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace {{ project.namespace }}.EntityFrameworkCore;

public static class {{ entity.name }}DbContextModelCreatingExtensions
{
    public static void Configure{{ entity.name }}(this ModelBuilder builder)
    {
        builder.Entity<{{ entity.name }}>(b =>
        {
            b.ToTable({{ project.name }}Consts.DbTablePrefix + "{{ entity.pluralName }}", {{ project.name }}Consts.DbSchema);
            b.ConfigureByConvention();

            {%- for field in entity.fields %}
            {%- if field.type == 'string' and field.maxLength %}
            b.Property(x => x.{{ field.name }}).HasMaxLength({{ field.maxLength }});
            {%- endif %}
            {%- if field.isRequired and field.type == 'string' %}
            b.Property(x => x.{{ field.name }}).IsRequired();
            {%- endif %}
            {%- endfor %}

            // ========== Relationship Configuration (1:N) ==========
            {%- for rel in relationships.asChild %}
            b.HasOne<{{ rel.parentEntityName }}>()
                .WithMany(p => p.{{ rel.parentNavigationName }})
                .HasForeignKey(x => x.{{ rel.fkFieldName }})
                {%- if rel.isRequired %}
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                {%- else %}
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
                {%- endif %}
            {%- endfor %}
        });
    }
}
`;
}

/**
 * DbContext property template (snippet to add to DbContext)
 */
export function getDbContextPropertyTemplate(): string {
    return `    public DbSet<{{ entity.name }}> {{ entity.pluralName }} { get; set; }
`;
}
