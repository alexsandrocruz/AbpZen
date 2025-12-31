import { APP_INITIALIZER, inject } from '@angular/core';
import { AuthErrorFilter, AuthErrorEvent, AuthErrorFilterService } from '@abp/ng.core';
import { eSaasAuthFilterNames } from '../enums';

export const SAAS_AUTH_FILTER_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureAuthFilter, multi: true },
];

type Reason = object & { error: { grant_type: string | undefined } };

function configureAuthFilter() {
  const errorFilterService = inject(
    AuthErrorFilterService<AuthErrorFilter<AuthErrorEvent>, AuthErrorEvent>,
  );
  const filter: AuthErrorFilter = {
    id: eSaasAuthFilterNames.Impersonation,
    executable: true,
    execute: (event: AuthErrorEvent) => {
      const { reason } = event;
      const {
        error: { grant_type },
      } = <Reason>(reason || {});

      return !!grant_type && grant_type === eSaasAuthFilterNames.Impersonation;
    },
  };

  return () => errorFilterService.add(filter);
}
