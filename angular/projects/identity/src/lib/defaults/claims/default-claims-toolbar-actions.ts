import { ToolbarAction } from '@abp/ng.components/extensible';
import { ClaimTypeDto } from '@volo/abp.ng.identity/proxy';
import { ClaimsComponent } from '../../components/claims/claims.component';

export const DEFAULT_CLAIMS_TOOLBAR_ACTIONS = ToolbarAction.createMany<ClaimTypeDto[]>([
  {
    text: 'AbpIdentity::NewClaimType',
    action: data => {
      const component = data.getInjected(ClaimsComponent);
      component.onAdd();
    },
    permission: 'AbpIdentity.ClaimTypes.Create',
    icon: 'fa fa-plus',
  },
]);
