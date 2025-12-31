import { FormControl } from "@angular/forms";

export interface RegisterFormModel{
  username: FormControl<string>;
  password: FormControl<string>;
  email: FormControl<string>;
}