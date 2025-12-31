using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.AuditLogging.Blazor.Components;

public partial class EntityChangeHistoryModal
{
    [Inject]
    private IAuditLogsAppService AppService { get; set; }

    private Modal _modal;

    protected List<EntityChangeWithUsernameDto> EntityHistories;

    protected Dictionary<Guid, bool> EntityChangesPanelStatus;

    protected string EntityTypeFullName;

    protected string EntityId;

    public virtual async Task OpenAsync(Guid id)
    {
        EntityChangesPanelStatus = new Dictionary<Guid, bool>();

        var entityChange = await AppService.GetEntityChangeWithUsernameAsync(id);

        EntityHistories = new List<EntityChangeWithUsernameDto>
            {
                entityChange
            };

        await _modal.Show();
    }

    public virtual async Task OpenAsync(string entityTypeFullName, string entityId)
    {
        EntityChangesPanelStatus = new Dictionary<Guid, bool>();
        EntityTypeFullName = entityTypeFullName;
        EntityId = entityId;
        EntityHistories = await AppService.GetEntityChangesWithUsernameAsync(new EntityChangeFilter
        {
            EntityTypeFullName = entityTypeFullName,
            EntityId = entityId
        });

        await _modal.Show();
    }

    private async Task CloseModal()
    {
        await _modal.Hide();
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual string GetValueInProperFormat(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }
        
        if(!DateTime.TryParse(value.Trim('"'), out var formattedValue))
        {
            return value;
        }

        return formattedValue.ToString();
    }
}
