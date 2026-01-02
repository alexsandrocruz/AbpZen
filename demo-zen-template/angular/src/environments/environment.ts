import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'LeptonXDemoApp',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44322/',
    redirectUri: baseUrl,
    clientId: 'LeptonXDemoApp_App',
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
