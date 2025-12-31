import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BodyService {
  body = document.querySelector('body');
  classes = {
    overflowYHidden: 'overflow-y-hidden',
  };

  disableScrollY() {
    this.body?.classList.add(this.classes.overflowYHidden);
  }

  enableScrollY() {
    this.body?.classList.remove(this.classes.overflowYHidden);
  }
}
