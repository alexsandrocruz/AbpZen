/**
 * Menu contributor additions template
 */
export function getMenuContributorTemplate(): string {
    return `using System.Threading.Tasks;
using {{ project.namespace }}.Localization;
using {{ project.namespace }}.Permissions;
using Volo.Abp.UI.Navigation;

namespace {{ project.namespace }}.Web.Menus;

public class {{ entity.name }}MenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<{{ project.name }}Resource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                {{ project.name }}Menus.{{ entity.name }},
                l["Menu:{{ entity.name }}"],
                url: "/{{ entity.pluralName }}",
                icon: "fas fa-list",
                requiredPermissionName: {{ project.name }}Permissions.{{ entity.name }}.Default
            )
        );
    }
}
`;
}

/**
 * Menu name constant
 */
export function getMenuNameTemplate(): string {
    return `    public const string {{ entity.name }} = Prefix + ".{{ entity.name }}";
`;
}

/**
 * Menu item snippet (to add to existing menu contributor)
 */
export function getMenuItemSnippet(): string {
    return `
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    {{ project.name }}Menus.{{ entity.name }},
                    l["Menu:{{ entity.pluralName }}"],
                    url: "/{{ entity.pluralName | kebabCase }}",
                    icon: "fas fa-list",
                    requiredPermissionName: {{ project.name }}Permissions.{{ entity.name }}.Default
                )
            );
`;
}
