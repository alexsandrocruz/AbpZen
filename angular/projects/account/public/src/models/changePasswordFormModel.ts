import { FormControl } from "@angular/forms";

export interface ChangePasswordFormModel{
  currentPassword: FormControl<string>;
  newPassword: FormControl<string>;
  repeatNewPassword: FormControl<string>;
}