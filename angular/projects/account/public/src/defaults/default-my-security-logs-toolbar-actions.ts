import { ToolbarAction } from '@abp/ng.components/extensible';
import { IdentitySecurityLogDto } from '@volo/abp.commercial.ng.ui/config';

export const DEFAULT_MY_SECURITY_LOGS_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  IdentitySecurityLogDto[]
>([]);
