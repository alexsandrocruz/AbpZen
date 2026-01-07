import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Sapienza.Zen',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44322/',
    redirectUri: baseUrl,
    clientId: 'Sapienza.Zen_App',
    responseType: 'code',
    scope: 'offline_access Sapienza.Zen',
    requireHttps: true,
    impersonation: {
      userImpersonation: true,
      tenantImpersonation: true,
    },
  },
  apis: {
    default: {
      url: 'https://localhost:44322',
      rootNamespace: 'Sapienza.Zen',
    },
  },
} as Environment;
