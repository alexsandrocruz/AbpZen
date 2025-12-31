import { APP_INITIALIZER, Provider } from '@angular/core';

export const LPX_STYLE_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initStyles,
  multi: true,
};

export function initStyles() {
  function init() {
    const loader = document.querySelector('#lp-page-loader') as HTMLElement;
    if (loader) {
      loader.style.background = 'var(--background)';
      setTimeout(() => loader.parentNode?.removeChild(loader), 500);
    }
    return Promise.resolve();
  }

  return init;
}
