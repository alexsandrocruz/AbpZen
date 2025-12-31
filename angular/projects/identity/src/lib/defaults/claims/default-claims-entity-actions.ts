import { EntityAction } from '@abp/ng.components/extensible';
import { ClaimTypeDto } from '@volo/abp.ng.identity/proxy';
import { ClaimsComponent } from '../../components/claims/claims.component';

export const DEFAULT_CLAIMS_ENTITY_ACTIONS = EntityAction.createMany<ClaimTypeDto>([
  {
    text: 'AbpUi::Edit',
    action: data => {
      const component = data.getInjected(ClaimsComponent);
      component.onEdit(data.record.id);
    },
    permission: 'AbpIdentity.ClaimTypes.Update',
    visible: data => !data.record.isStatic,
  },
  {
    text: 'AbpUi::Delete',
    action: data => {
      const component = data.getInjected(ClaimsComponent);
      component.delete(data.record.id, data.record.name);
    },
    permission: 'AbpIdentity.ClaimTypes.Delete',
  },
]);
