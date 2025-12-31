import { ConfigStateService, ListService, PagedResultDto, SessionStateService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { Component, inject, Injector, OnInit } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import {
  CultureInfoDto,
  GetLanguagesTextsInput,
  LanguageDto,
  LanguageService,
} from '@volo/abp.ng.language-management/proxy';
import { of } from 'rxjs';
import { finalize, map, switchMap } from 'rxjs/operators';
import { eLanguageManagementComponents } from '../../enums/components';
import flagIcons from './flag-icons';

@Component({
  selector: 'abp-languages',
  templateUrl: './languages.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eLanguageManagementComponents.Languages,
    },
  ],
})
export class LanguagesComponent implements OnInit {
  protected readonly list = inject(ListService<GetLanguagesTextsInput>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly service = inject(LanguageService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly sessionService = inject(SessionStateService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly injector = inject(Injector);

  data: PagedResultDto<LanguageDto> = { items: [], totalCount: 0 };

  cultures$ = of([] as CultureInfoDto[]);

  form: UntypedFormGroup;

  selected: LanguageDto;

  isModalVisible = false;

  modalBusy = false;

  flagIcons = flagIcons;

  protected get controls() {
    return this.form.controls;
  }

  protected createForm(): void {
    this.cultures$
      .pipe(switchMap(cultures => (cultures.length ? of(cultures) : this.service.getCulturelist())))
      .subscribe(res => {
        this.cultures$ = of(res);

        const data = new FormPropData(this.injector, this.selected);
        this.form = generateFormFromProps(data);

        this.controls?.cultureName?.patchValue(null);
        this.controls?.uiCultureName?.patchValue(null);
      });
  }

  protected hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }

  ngOnInit() {
    this.hookToQuery();
  }

  openModal() {
    this.createForm();
    this.isModalVisible = true;
  }

  add() {
    this.selected = {} as LanguageDto;
    this.openModal();
  }

  edit(id: string) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.openModal();
    });
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;

    const { id } = this.selected;
    const input = { ...this.form.value };

    if (!input.displayName) {
      input.displayName = input.cultureName;
    }

    (id ? this.service.update(id, { ...this.selected, ...input }) : this.service.create(input))
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.list.get();
        if (!this.selected.id) {
          //TODO refactor for nested subscription
          this.configState.refreshAppState().subscribe();
        }
      });
  }

  delete(id: string, name: string, isDefaultLanguage = false) {
    let warningMessageKey = 'LanguageManagement::LanguageDeletionConfirmationMessage';
    if (isDefaultLanguage) {
      warningMessageKey = 'LanguageManagement::DefaultLanguageDeletionConfirmationMessage';
    }
    this.confirmationService
      .warn(warningMessageKey, 'LanguageManagement::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.get(id).subscribe(lang => {
            this.service.delete(id).subscribe(() => {
              let defaultLanguage: LanguageDto;
              this.service.getAllList().subscribe(list => {
                list.items.forEach(item => {
                  if (item.isDefaultLanguage) {
                    defaultLanguage = item;
                  }
                });
                if (lang.cultureName == this.sessionService.getLanguage()) {
                  this.sessionService.setLanguage(defaultLanguage.cultureName);
                }
              });
              this.toasterService.success('AbpUi::DeletedSuccessfully');
              this.list.get();
            });
          });
        }
      });
  }

  setAsDefault(id: string) {
    this.service.setAsDefault(id).subscribe(() => this.list.get());
  }
}
