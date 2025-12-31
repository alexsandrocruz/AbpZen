import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'abp-layout-empty',
  standalone: true,
  imports: [RouterModule],
  template: ` <router-outlet></router-outlet> `,
})
export class EmptyLayoutComponent {
  static type = eLayoutType.empty;
}
