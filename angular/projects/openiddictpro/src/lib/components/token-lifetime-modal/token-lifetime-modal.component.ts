import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Applications, ApplicationService } from '@volo/abp.ng.openiddictpro/proxy';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { ExtensibleFormComponent } from '@abp/ng.components/extensible';
import { FormBuilder } from '@angular/forms';
import { CoreModule } from '@abp/ng.core';
import { ApplicationsService } from '../../services';
import { finalize } from 'rxjs';

@Component({
  selector: 'abp-token-lifetime-modal',
  standalone: true,
  imports: [CommonModule, ThemeSharedModule, CoreModule, ExtensibleFormComponent],
  templateUrl: './token-lifetime-modal.component.html',
})
export class TokenLifetimeModalComponent implements OnInit {
  #service = inject(ApplicationsService);
  #proxyService = inject(ApplicationService);
  @Input()
  selected: Applications.Dtos.ApplicationDto;
  @Output()
  saved = new EventEmitter();
  loading = signal(false);

  #fb = inject(FormBuilder);

  protected readonly toasterService = inject(ToasterService);
  form = this.#fb.group({
    accessTokenLifetime: [null as number],
    authorizationCodeLifetime: [null as number],
    deviceCodeLifetime: [null as number],
    identityTokenLifetime: [null as number],
    refreshTokenLifetime: [null as number],
    userCodeLifetime: [null as number],
  });

  save() {
    if (this.form.invalid) {
      return;
    }

    const id = this.selected.id;
    this.#proxyService
      .setTokenLifetime(id, this.form.value)
      .pipe(
        finalize(() => {
          this.#service.setTokenLifetimeModalState(false);
          this.toasterService.success('AbpUi::SavedSuccessfully');
          this.saved.emit();
        }),
      )
      .subscribe();
  }

  visibleChange($event: boolean) {
    if ($event) {
      return;
    }
    this.#service.setTokenLifetimeModalState(false);
  }

  ngOnInit(): void {
    this.#proxyService.getTokenLifetime(this.selected.id).subscribe(x => {
      this.form.patchValue(x);
    });
  }
}
