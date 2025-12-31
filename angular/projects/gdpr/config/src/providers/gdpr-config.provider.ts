import { Provider, makeEnvironmentProviders } from '@angular/core';
import {
  GDPR_ROUTE_PROVIDERS,
  GDPR_USER_MENU_ITEM_PROVIDERS,
  cookieConsentOptionsProvider,
} from './';
import { AbpCookieConsentOptions } from '../models';

export enum GdprFeatureKind {
  CookieConsentOptions,
}

export interface GdprFeature<KindT extends GdprFeatureKind> {
  ɵkind: KindT;
  ɵproviders: Provider[];
}

function makeGdprFeature<KindT extends GdprFeatureKind>(
  kind: KindT,
  providers: Provider[],
): GdprFeature<KindT> {
  return {
    ɵkind: kind,
    ɵproviders: providers,
  };
}

export function withCookieConsentOptions(
  options?: AbpCookieConsentOptions,
): GdprFeature<GdprFeatureKind.CookieConsentOptions> {
  return makeGdprFeature(GdprFeatureKind.CookieConsentOptions, [
    cookieConsentOptionsProvider(options),
  ]);
}

export function provideGdprConfig(...features: GdprFeature<GdprFeatureKind>[]) {
  const providers = [
    GDPR_USER_MENU_ITEM_PROVIDERS,
    GDPR_ROUTE_PROVIDERS,
    cookieConsentOptionsProvider(),
  ];

  for (const feature of features) {
    providers.push(...feature.ɵproviders);
  }

  return makeEnvironmentProviders([providers]);
}
