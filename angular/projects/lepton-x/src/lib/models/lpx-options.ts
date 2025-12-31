import { HasStyleFactory, LpxCoreOptions } from '@volo/ngx-lepton-x.core';
import { LpxThemeModuleOptions } from '../theme';
import { LpxHttpError } from './http-error';

export interface LpxOptions extends LpxCoreOptions, HasStyleFactory {
  themeOptions?: LpxThemeModuleOptions;
  defaultTheme?: string;
  httpError?: LpxHttpError;
}
