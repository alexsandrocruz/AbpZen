import { ChangeDetectorRef, Component, inject, Injector, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ConfigStateService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { AbstractAccountSettingsService } from './abstract-account-config.service';

@Component({ template: '' })
export class AbstractAccountSettingsComponent<Type, SubmitType = Type> implements OnInit { 
  protected readonly service: AbstractAccountSettingsService<Type, SubmitType> = inject(
    AbstractAccountSettingsService,
  );
  protected readonly toasterService: ToasterService = inject(ToasterService);
  protected readonly cdr: ChangeDetectorRef = inject(ChangeDetectorRef);
  protected readonly configState: ConfigStateService = inject(ConfigStateService);

  @Input() isTenant: boolean;

  settings$: Observable<Type>;

  private _loading: boolean;
  set loading(value: boolean) {
    this._loading = value;
    this.cdr.markForCheck();
  }

  get loading() {
    return this._loading;
  }

  ngOnInit() {
    this.settings$ = this.service.getSettings();
  }

  submit(newSettings: Partial<SubmitType>) {
    this.loading = true;
    this.service
      .updateSettings(this.isTenant ? this.mapTenantSettingsForSubmit(newSettings) : newSettings)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully', null);
        this.configState.refreshAppState().subscribe();
      });
  }

  /**
   * should be overriden by children components
   * if it is not overridden,
   * it means that there is no difference between host and tenant for the particular child
   */
  mapTenantSettingsForSubmit(newSettings: Partial<SubmitType>) {
    return newSettings;
  }
}
