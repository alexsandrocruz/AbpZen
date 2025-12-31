import { VariantSource } from '@volo/ngx-lepton-x.core';
import { TemplateRef, Type } from '@angular/core';
import { SettingsService } from './settings-service.model';
import { ContextMenuTriggerEvent } from '../../models';

export interface Setting {
  text?: string;
  icon?: string;
  shortText?: string;
  action?: () => VariantSource<boolean>;
  selected?: boolean;
  expanded?: boolean;
  order?: number;
  children?: Setting[];
  visibleInCtxMenu?: boolean;
  id?: string;
  template?: TemplateRef<any>;
}

export interface SettingsOptions {
  generalSettingsTranslateKey: string;
  settingsService: Type<SettingsService>;
  settingsMenuEventName?: ContextMenuTriggerEvent;
}
