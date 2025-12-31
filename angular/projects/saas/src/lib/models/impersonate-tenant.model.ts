import { FormControl } from '@angular/forms';

export type ImpersonateTenantModalFormGroupModel = {
  tenantUserName: FormControl<string>;
};

export type ImpersonateTenantModalState = {
  tenantId: string | null;
  isVisible?: boolean;
};
