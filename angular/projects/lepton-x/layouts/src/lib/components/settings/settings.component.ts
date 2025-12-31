import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  Inject,
  QueryList,
  ViewChild,
  ViewChildren,
  ViewEncapsulation,
} from '@angular/core';
import { startWith } from 'rxjs/operators';
import { ContextMenuComponent } from '@volosoft/ngx-lepton-x';
import { SettingComponent } from './setting.component';
import { Setting } from './setting.model';
import { SettingsService } from './settings-service.model';
import {
  LPX_GENERAL_SETTINGS_TRANSLATE_KEY,
  LPX_SETTINGS_MENU_EVENT_NAME,
  LPX_SETTINGS_SERVICE,
} from '../../tokens';
import { ContextMenuTriggerEvent } from '../../models';

@Component({
  selector: 'lpx-settings',
  templateUrl: './settings.component.html',
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SettingsComponent implements AfterViewInit {
  @ViewChildren(SettingComponent, { read: ElementRef })
  settingComponents!: QueryList<ElementRef>;

  exceptedRefs: Array<ElementRef> = [];
  @ViewChild(ContextMenuComponent)
  ctxMenu!: ContextMenuComponent;
  constructor(
    @Inject(LPX_SETTINGS_SERVICE) public service: SettingsService,
    @Inject(LPX_GENERAL_SETTINGS_TRANSLATE_KEY)
    public generalSettingsTranslateKey: string,
    @Inject(LPX_SETTINGS_MENU_EVENT_NAME)
    private menuTriggerType: ContextMenuTriggerEvent,
    private cdRef: ChangeDetectorRef
  ) {}

  hideCtxMenu(): void {
    this.ctxMenu.close();
  }

  toggleCtxMenu(settings: Setting[]): void {
    this.ctxMenu.toggle();
    if (this.ctxMenu.visible) {
      settings.forEach((s) => (s.expanded = true));
    }
  }

  openCtxMenuWith(settings: Setting[], setting: Setting) {
    if (this.menuTriggerType === 'click') {
      this.ctxMenu.open();
    }
    settings.forEach((s) => {
      s.expanded = s.id === setting.id;
    });
  }

  ngAfterViewInit(): void {
    this.settingComponents.changes
      .pipe(startWith(this.settingComponents.toArray()))
      .subscribe((changes) => {
        this.exceptedRefs = changes;
        this.cdRef.detectChanges();
      });
  }

  openAllExpendedSettingsPanel(settings: Setting[]) {
    if (this.menuTriggerType === 'click') {
      return;
    }
    settings.forEach((s) => {
      s.expanded = true;
    });
  }

  openExpendedSettingsPanel(
    settings: Setting[],
    selectedSetting: Setting,
    eventName: string
  ) {
    if (this.menuTriggerType !== eventName) {
      return;
    }
    settings.forEach((s) => {
      s.expanded = s.id === selectedSetting.id;
    });
  }

  openMenu(eventName: string) {
    if (this.menuTriggerType !== eventName) {
      return;
    }
    this.ctxMenu.open();
  }

  closeMenu(eventName: string) {
    if (this.menuTriggerType !== eventName) {
      return;
    }
    this.ctxMenu.close();
  }
}
