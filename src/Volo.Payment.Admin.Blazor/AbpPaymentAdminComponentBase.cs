using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin.Blazor;

public abstract class AbpPaymentAdminComponentBase : AbpComponentBase
{
    protected ExtensionPropertyPolicyChecker ExtensionPropertyPolicyChecker => ScopedServices.GetRequiredService<ExtensionPropertyPolicyChecker>();
    protected IAbpEnumLocalizer AbpEnumLocalizer => ScopedServices.GetRequiredService<IAbpEnumLocalizer>();

    protected AbpPaymentAdminComponentBase()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(AbpPaymentAdminBlazorModule);
    }

    protected virtual async Task<List<TableColumn>> GetExtensionTableColumnsAsync(string moduleName, string entityType)
    {
        var tableColumns = new List<TableColumn>();
        var properties = ModuleExtensionConfigurationHelper.GetPropertyConfigurations(moduleName, entityType).ToList();
        foreach (var propertyInfo in properties)
        {
            if (!await ExtensionPropertyPolicyChecker.CheckPolicyAsync(propertyInfo.Policy))
            {
                continue;
            }

            if (propertyInfo.IsAvailableToClients && propertyInfo.UI.OnTable.IsVisible)
            {
                if (propertyInfo.Name.EndsWith("_Text"))
                {
                    var lookupPropertyName = propertyInfo.Name.RemovePostFix("_Text");
                    var lookupPropertyDefinition = properties.SingleOrDefault(t => t.Name == lookupPropertyName)!;
                    tableColumns.Add(new TableColumn
                    {
                        Title = lookupPropertyDefinition.GetLocalizedDisplayName(StringLocalizerFactory),
                        Data = $"ExtraProperties[{propertyInfo.Name}]",
                        PropertyName = propertyInfo.Name
                    });
                }
                else
                {
                    var column = new TableColumn
                    {
                        Title = propertyInfo.GetLocalizedDisplayName(StringLocalizerFactory),
                        Data = $"ExtraProperties[{propertyInfo.Name}]",
                        PropertyName = propertyInfo.Name
                    };

                    if (propertyInfo.IsDate() || propertyInfo.IsDateTime())
                    {
                        column.DisplayFormat = propertyInfo.GetDateEditInputFormatOrNull();
                    }

                    if (propertyInfo.Type.IsEnum)
                    {
                        column.ValueConverter = (val) =>
                            AbpEnumLocalizer.GetString(propertyInfo.Type, val.As<ExtensibleObject>().ExtraProperties[propertyInfo.Name]!, new IStringLocalizer?[]{ StringLocalizerFactory.CreateDefaultOrNull() });
                    }

                    tableColumns.Add(column);
                }
            }
        }

        return tableColumns;
    }
}
