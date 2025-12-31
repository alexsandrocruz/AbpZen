import { APP_INITIALIZER } from '@angular/core';
import {
  createStyleFactory,
  LPX_INITIAL_STYLES,
  LPX_LAYOUT_STYLE_FINAL,
  LPX_STYLE_FINAL,
  LpxStyleFactory,
  styleLoadFactory,
} from '@volo/ngx-lepton-x.core';
import { LPX_PRO_STYLE_TOKEN } from '../tokens/style.token';

// TODO: make generator function in core
export const getLpxProStyleProviders = (styleFactory?: LpxStyleFactory) => [
  {
    provide: APP_INITIALIZER,
    deps: [LPX_PRO_STYLE_TOKEN],
    multi: true,
    useValue: () => null,
  },
  {
    provide: LPX_PRO_STYLE_TOKEN,
    deps: [LPX_INITIAL_STYLES, LPX_LAYOUT_STYLE_FINAL],
    useFactory: styleLoadFactory,
  },
  {
    provide: LPX_STYLE_FINAL,
    deps: [LPX_PRO_STYLE_TOKEN],
    useFactory: createStyleFactory(styleFactory),
  },
];
