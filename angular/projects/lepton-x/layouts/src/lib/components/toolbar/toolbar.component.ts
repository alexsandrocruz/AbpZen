import {
  AfterViewInit,
  Component,
  ContentChild,
  ElementRef,
  EventEmitter,
  Inject,
  Input,
  Output,
  TemplateRef,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {
  AuthService,
  LPX_AUTH_SERVICE_TOKEN,
  ToolbarService,
  UserProfileService,
} from '@volo/ngx-lepton-x.core';
import { ToolbarItemsDirective } from './toolbar-items/toolbar-items.directive';

@Component({
  selector: 'lpx-toolbar',
  templateUrl: './toolbar.component.html',
  styles: [],
  encapsulation: ViewEncapsulation.None,
})
export class ToolbarComponent implements AfterViewInit {
  @Input()
  profileRef?: ReplaySubject<ElementRef>;

  @Output()
  profileClick = new EventEmitter<void>();

  @ViewChild('profileLink', { static: false })
  profileLink!: ElementRef;

  @ContentChild(ToolbarItemsDirective, { read: TemplateRef })
  itemsTmp?: TemplateRef<any>;

  constructor(
    public readonly userProfileService: UserProfileService,
    public readonly toolbarService: ToolbarService,
    @Inject(LPX_AUTH_SERVICE_TOKEN) public readonly authService: AuthService
  ) {}

  ngAfterViewInit(): void {
    if (this.profileRef) {
      this.profileRef.next(this.profileLink);
    }
  }

  navigateToLogin() {
    this.authService.navigateToLogin();
  }
}
