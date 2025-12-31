import { Optional, SkipSelf } from '@angular/core';
import { ControlContainer } from '@angular/forms';

export const containerFactory = (container: ControlContainer) => {
  if (!container) {
    throw new Error('I need a FormGroup instance');
  }
  return container;
};

export const parentFormGroupProvider = [
  {
    provide: ControlContainer,
    useFactory: containerFactory,
    deps: [[new SkipSelf(), new Optional(), ControlContainer]],
  },
];
