import { HasStyleFactory, LpxStyle } from '@volo/ngx-lepton-x.core';

export interface LpxThemeOptions {
  localStorageKey: string;
  disableSystemOption?: boolean;
}

export type SystemKey = 'dark' | 'light';

export interface LpxThemeStyleConfig {
  bundles: LpxStyle[];
  styleName: string;
  defaultTheme?: boolean;
  label: string;
  icon: string;
  order?: number;
  systemKey?: SystemKey;
}

export class LpxTheme implements LpxThemeStyleConfig {
  bundles: LpxStyle[];
  styleName: string;
  defaultTheme?: boolean;
  label: string;
  icon: string;
  order?: number;
  systemKey?: SystemKey;
  constructor(args: LpxThemeStyleConfig) {
    this.bundles = args.bundles;
    this.styleName = args.styleName;
    this.defaultTheme = args.defaultTheme;
    this.label = args.label;
    this.icon = args.icon;
    this.order = args.order;
    this.systemKey = args.systemKey;
  }

  updateBundle(bundle: LpxStyle) {
    const updateIndex = this.bundles.findIndex(
      (b) => bundle.bundleName === b.bundleName
    );
    if (updateIndex > -1) {
      this.bundles[updateIndex] = bundle;
    }
  }

  addBundle(bundle: LpxStyle) {
    this.bundles.push(bundle);
  }

  deleteBundle(bundle: LpxStyle) {
    const deleteIndex = this.bundles.findIndex(
      (b) => bundle.bundleName === b.bundleName
    );
    if (deleteIndex > -1) {
      this.bundles.splice(deleteIndex, 1);
    }
  }
}
export type LpxThemes = Array<LpxTheme>;

export interface LpxThemeModuleOptions
  extends LpxThemeOptions,
    HasStyleFactory<LpxThemes> {}
