import { ChangeDetectorRef, Directive, Input, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize, switchMap } from 'rxjs/operators';
import { ConfigStateService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { AuditLogSettingsService } from '@volo/abp.ng.audit-logging/proxy';

@Directive({ standalone: true })
export abstract class AbstractAuditLogSettingsComponent implements OnInit {
  protected readonly cdr = inject(ChangeDetectorRef);
  protected readonly fb = inject(FormBuilder);
  protected readonly toasterService = inject(ToasterService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly settingsService = inject(AuditLogSettingsService);

  globalTab = false;

  today = new Date();

  //TODO: make it a signal input
  @Input() isTenant: boolean;

  //TODO: Try to use type safety
  form: FormGroup = this.fb.group({
    isExpiredDeleterEnabled: false,
    expiredDeleterPeriod: 0,
  });

  private _loading: boolean;
  set loading(value: boolean) {
    this._loading = value;
    this.cdr.markForCheck();
  }

  get loading() {
    return this._loading;
  }

  protected buildForm() {
    let req = this.settingsService.get();
    if (this.globalTab && !this.isTenant) {
      req = this.settingsService.getGlobal();
      this.form.addControl('isPeriodicDeleterEnabled', this.fb.control(false));
    }

    req.subscribe(settings => this.form.setValue(settings));
  }

  protected adjustFormValues() {
    const controls = this.form.controls;
    if (!controls.isExpiredDeleterEnabled.value) {
      controls.expiredDeleterPeriod.setValue(0);
    }
  }

  ngOnInit() {
    this.buildForm();
  }

  submit() {
    this.loading = true;
    this.adjustFormValues();

    const input = this.form.value;

    //TODO: Improve this logic, we should not need this check
    if (!input.expiredDeleterPeriod) {
      input.expiredDeleterPeriod = 0;
    }

    let req = this.settingsService.update(input);
    if (this.globalTab && !this.isTenant) {
      req = this.settingsService.updateGlobal(input);
    }

    req
      .pipe(
        switchMap(() => this.configState.refreshAppState()),
        finalize(() => (this.loading = false)),
      )
      .subscribe(() => this.toasterService.success('AbpUi::SavedSuccessfully'));
  }

  getDateBeforeToday(day: number) {
    const date = new Date(this.today);
    date.setDate(date.getDate() - day);
    return date;
  }
}
