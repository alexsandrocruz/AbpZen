import { Component, inject, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Scopes, ScopeService } from '@volo/abp.ng.openiddictpro/proxy';
import { eOpenIddictProComponents } from '../../enums/components';
import { ScopesService } from '../../services/scopes.service';

@Component({
  selector: 'abp-scopes',
  templateUrl: './scopes.component.html',
  styles: [],
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eOpenIddictProComponents.Scopes,
    },
  ],
})
export class ScopesComponent implements OnInit {
  protected readonly list = inject(ListService<Scopes.Dtos.GetScopeListInput>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly service = inject(ScopeService);
  protected readonly scopesService = inject(ScopesService);

  data: PagedResultDto<Scopes.Dtos.ScopeDto> = { items: [], totalCount: 0 };
  selected = {} as Scopes.Dtos.ScopeDto;

  private hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }
  ngOnInit() {
    this.hookToQuery();
  }
  onAdd() {
    this.selected = {} as Scopes.Dtos.ScopeDto;
    this.scopesService.openModal();
  }
  onEdit(id) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.scopesService.openModal();
    });
  }

  onDelete(id, name) {
    const sub = this.confirmationService
      .warn('AbpOpenIddict::ScopeDeletionWarningMessage', 'AbpOpenIddict::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(id).subscribe(() => {
            this.toasterService.success('AbpUi::DeletedSuccessfully');
            this.list.get();
          });
        }
      });
  }
  onSaved() {
    this.list.get();
  }
}
