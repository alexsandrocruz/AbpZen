import { ChangeDetectionStrategy, Component, ViewEncapsulation } from '@angular/core';
import { Validation, ValidationErrorComponent } from '@ngx-validate/core';
@Component({
  selector: 'abp-validation-error',
  // TODO: add validation error class to span
  template: `
    <span class="text-danger" data-valmsg-for="Role.Name" data-valmsg-replace="true"
      ><span *ngFor="let error of abpErrors; trackBy: trackByFn">
        {{ error.message | abpLocalization: error.interpoliteParams }}</span
      ></span
    >
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [`:host-context {order:3}`]
})
export class AbpValidationErrorComponent extends ValidationErrorComponent {
  get abpErrors(): Validation.Error[] & { interpoliteParams?: string[] } {
    if (!this.errors || !this.errors.length) {
      return [];
    }

    return this.errors.map(error => {
      if (!error.message) {
        return error;
      }

      const index = error.message.indexOf('[');

      if (index > -1) {
        return {
          ...error,
          message: error.message.slice(0, index),
          interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(','),
        };
      }

      return error;
    });
  }
}
