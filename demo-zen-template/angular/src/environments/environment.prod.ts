import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'LeptonXDemoApp',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44322',
    redirectUri: baseUrl,
    clientId: 'LeptonXDemoApp',
    responseType: 'code',
    scope: 'offline_access LeptonXDemoApp',
    requireHttps: true,
    impersonation: {
      userImpersonation: true,
      tenantImpersonation: true,
    },
  },
  apis: {
    default: {
      url: 'https://localhost:44322',
      rootNamespace: 'LeptonXDemoApp',
    },
  },
} as Environment;
