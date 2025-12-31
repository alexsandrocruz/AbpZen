import { Applications, ApplicationService } from '@volo/abp.ng.openiddictpro/proxy';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component, inject, OnInit, signal } from '@angular/core';
import { filter, switchMap } from 'rxjs';
import { eOpenIddictProComponents } from '../../enums/components';
import { ApplicationsService } from '../../services/applications.service';

@Component({
  selector: 'abp-applications',
  templateUrl: './applications.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eOpenIddictProComponents.Applications,
    },
  ],
})
export class ApplicationsComponent implements OnInit {
  protected readonly service = inject(ApplicationsService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly proxyService = inject(ApplicationService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly list: ListService<Applications.Dtos.GetApplicationListInput> =
    inject(ListService);

  data: PagedResultDto<Applications.Dtos.ApplicationDto> = { items: [], totalCount: 0 };
  readonly selected = signal({} as Applications.Dtos.ApplicationDto);
  providerKey: string;
  visiblePermissions = false;

  onVisiblePermissionChange = (value: boolean) => {
    this.visiblePermissions = value;
  };

  onSaved() {
    this.list.get();
  }

  edit(id: string) {
    this.proxyService.get(id).subscribe(res => {
      this.selected.set(res);
      this.service.openModal();
    });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpOpenIddict::ApplicationDeletionWarningMessage', 'AbpOpenIddict::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(id)),
      )
      .subscribe(() => {
        this.toasterService.success('AbpUi::DeletedSuccessfully');
        this.list.get();
      });
  }

  ngOnInit() {
    this.hookToQuery();
  }

  onAdd() {
    this.selected.set({} as Applications.Dtos.ApplicationDto);
    this.service.openModal();
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }

  protected hookToQuery() {
    this.list
      .hookToQuery(query => this.proxyService.getList(query))
      .subscribe(res => (this.data = res));
  }

  openTokenLifetimeModal(record: Applications.Dtos.ApplicationDto) {
    this.selected.set(record);
    this.service.setTokenLifetimeModalState(true);
  }
}
