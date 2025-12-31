import { APP_INITIALIZER } from '@angular/core';
import {
  createStyleFactory,
  LPX_LAYOUT_STYLE_FINAL,
  LpxStyleFactory,
} from '@volo/ngx-lepton-x.core';
import { LPX_TOP_MENU_LAYOUT_STYLE_TOKEN } from '../tokens';

export const getLpxTopMenuLayoutStyleProviders = (
  styleFactory?: LpxStyleFactory
) => [
  {
    provide: LPX_TOP_MENU_LAYOUT_STYLE_TOKEN,
    useFactory: () => {
      return [
        {
          bundleName: 'layout-bundle',
        },
      ];
    },
  },
  {
    provide: APP_INITIALIZER,
    deps: [LPX_TOP_MENU_LAYOUT_STYLE_TOKEN],
    useValue: () => null,
    multi: true,
  },
  {
    provide: LPX_LAYOUT_STYLE_FINAL,
    deps: [LPX_TOP_MENU_LAYOUT_STYLE_TOKEN],
    useFactory: createStyleFactory(styleFactory),
  },
];
