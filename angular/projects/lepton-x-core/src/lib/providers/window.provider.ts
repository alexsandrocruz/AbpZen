import { Provider } from '@angular/core';
import { WINDOW } from '../tokens/window.token';

export function createWindowProvider(windowObj: Window | undefined): Provider {
  return { provide: WINDOW, useValue: windowObj || window }
}
