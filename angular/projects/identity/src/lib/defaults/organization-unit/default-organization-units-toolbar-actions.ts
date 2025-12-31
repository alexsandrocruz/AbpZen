import { ToolbarAction } from '@abp/ng.components/extensible';
import { OrganizationUnitWithDetailsDto } from '@volo/abp.ng.identity/proxy';

export const DEFAULT_ORGANIZATION_UNITS_TOOLBAR_ACTIONS = ToolbarAction.createMany<
  OrganizationUnitWithDetailsDto[]
>([]);
