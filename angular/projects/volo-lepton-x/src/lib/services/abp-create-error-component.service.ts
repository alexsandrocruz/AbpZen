import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  CreateErrorComponentService,
  ErrorScreenErrorCodes,
  HttpErrorWrapperComponent,
} from '@abp/ng.theme.shared';
import { HTTP_ERROR_PATH } from '../tokens';

@Injectable()
export class AbpCreateErrorComponentService extends CreateErrorComponentService {
  protected readonly router = inject(Router);
  protected readonly httpErrorPath = inject(HTTP_ERROR_PATH);

  override execute(instance: Partial<HttpErrorWrapperComponent>): void {
    if (this.canCreateCustomError(instance.status as ErrorScreenErrorCodes)) {
      super.execute(instance);
      return;
    }

    const queryParams = { status: instance.status };
    this.router.navigate([this.httpErrorPath], { queryParams });
  }
}
