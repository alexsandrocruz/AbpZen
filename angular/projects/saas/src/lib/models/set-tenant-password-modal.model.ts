import { EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';

export interface SetPasswordFormGroupModel {
  adminName: FormControl<string>;
  password: FormControl<string>;
}

export interface SetTenantPasswordModalInputs {
  modalVisible: boolean;
  tenantId: string;
  tenantName: string;
}

export interface SetTenantPasswordModalOutputs {
  modalVisibleChange: EventEmitter<void>;
}
