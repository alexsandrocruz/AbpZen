import { Component, Input } from '@angular/core';
import { ProgressBarStats } from '../../models/password-complexity';

@Component({
  selector: 'abp-password-complexity-indicator',
  template: `
    <div
      [style.opacity]="progressBar?.width > 0 ? 1 : 0"
      [style.backgroundColor]="'var(--lpx-border-color)'"
      class="progress transition mx-3"
    >
      <div
        class="progress-bar transition"
        [style.width]="progressBar?.width + '%'"
        [style.backgroundColor]="progressBar?.bgColor"
      ></div>
    </div>
  `,
  styles: [
    `
      .transition {
        transition: all 0.3s ease-out;
      }
      .progress {
        backgroundcolor: var(--lpx-border-color);
        height: 4px;
        border-radius: 3px 3px 0 0;
        margin-top: -5px;
        z-index: 1;
        position: relative;
      }
      :host-context {
        order: 1;
      }
    `,
  ],
})
export class PasswordComplexityIndicatorComponent {
  @Input({ required: true }) progressBar?: ProgressBarStats;
}
