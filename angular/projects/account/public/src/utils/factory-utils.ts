import { AccountConfigOptions } from '../models/config-options';

export function accountOptionsFactory(options: AccountConfigOptions) {
  return {
    redirectUrl: '/',
    ...(options as any), // any was added to avoid type error, please don't remove
  };
}
