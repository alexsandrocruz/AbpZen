import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Sapienza.Cursos',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44322',
    redirectUri: baseUrl,
    clientId: 'Sapienza.Cursos',
    responseType: 'code',
    scope: 'offline_access Sapienza.Cursos',
    requireHttps: true,
    impersonation: {
      userImpersonation: true,
      tenantImpersonation: true,
    },
  },
  apis: {
    default: {
      url: 'https://localhost:44322',
      rootNamespace: 'Sapienza.Cursos',
    },
  },
} as Environment;
