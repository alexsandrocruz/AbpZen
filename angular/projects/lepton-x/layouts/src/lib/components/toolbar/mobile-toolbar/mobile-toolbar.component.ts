import {
  ChangeDetectionStrategy,
  Component,
  ContentChild,
  EventEmitter,
  Input,
  Output,
  TemplateRef,
  TrackByFunction,
} from '@angular/core';
import { AsyncPipe, NgTemplateOutlet } from '@angular/common';
import { ToolbarItem } from '../models/toolbar';
import {
  LpxVisibleDirective,
  ToObservableModule,
} from '@volo/ngx-lepton-x.core';
import { MobileToolbarItemComponent } from './mobile-toolbar-item.component';

@Component({
  standalone: true,
  selector: 'lpx-mobile-toolbar',
  templateUrl: './mobile-toolbar.component.html',
  imports: [
    AsyncPipe,
    NgTemplateOutlet,
    ToObservableModule,
    LpxVisibleDirective,
    MobileToolbarItemComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MobileToolbarComponent {
  @Input() items: ToolbarItem[] | undefined | null;
  @Output() routeClick = new EventEmitter<ToolbarItem>();

  @ContentChild('items') itemsRef: TemplateRef<ToolbarItem[]> | undefined;
  @ContentChild('item') itemRef: TemplateRef<ToolbarItem> | undefined;

  trackByFn: TrackByFunction<ToolbarItem> = (_, element) =>
    element.id || element.name;
}
