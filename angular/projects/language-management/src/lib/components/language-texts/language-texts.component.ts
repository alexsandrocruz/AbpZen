import { ConfigStateService, LanguageInfo, ListService, PagedResultDto } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { Component, inject, OnInit, Renderer2 } from '@angular/core';
import {
  GetLanguagesTextsInput,
  LanguageService,
  LanguageTextDto,
  LanguageTextService,
} from '@volo/abp.ng.language-management/proxy';
import { finalize } from 'rxjs/operators';
import { eLanguageManagementComponents } from '../../enums/components';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-language-texts',
  templateUrl: './language-texts.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eLanguageManagementComponents.LanguageTexts,
    },
  ],
})
export class LanguageTextsComponent implements OnInit {
  protected readonly list = inject(ListService<GetLanguagesTextsInput>);
  protected readonly renderer = inject(Renderer2);
  protected readonly configState = inject(ConfigStateService);
  protected readonly service = inject(LanguageTextService);
  protected readonly languageService = inject(LanguageService);
  protected readonly toasterService = inject(ToasterService);

  data: PagedResultDto<LanguageTextDto> = { items: [], totalCount: 0 };

  columns: { field: string; header: string }[];

  selected: LanguageTextDto;

  selectedIndex: number;

  pageQuery = {} as GetLanguagesTextsInput;

  isModalVisible = false;

  modalBusy = false;

  languages: LanguageInfo[];

  resources: { name?: string }[] = [];

  ngOnInit() {
    this.languages = this.configState.getDeep('localization.languages');

    this.languageService.getResources().subscribe(resources => {
      this.resources = resources;
    });

    this.pageQuery = {
      baseCultureName: this.languages[0].cultureName,
      targetCultureName: this.languages?.[1]?.cultureName || this.languages?.[0]?.cultureName,
      getOnlyEmptyValues: false,
      resourceName: null,
    } as GetLanguagesTextsInput;

    this.hookToQuery();

    this.columns = [
      { field: 'name', header: 'LanguageManagement::Key' },
      { field: 'baseValue', header: 'LanguageManagement::BaseValue' },
      { field: 'value', header: 'LanguageManagement::Value' },
      { field: 'resourceName', header: 'LanguageManagement::ResourceName' },
    ];
  }

  openModal() {
    this.isModalVisible = true;
  }

  closeModal() {
    this.isModalVisible = false;
    this.selected = {} as LanguageTextDto;
    this.selectedIndex = null;
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => this.service.getList({ ...query, ...this.pageQuery }))
      .subscribe(res => {
        this.data = res;
        if (this.isModalVisible) {
          if (!res.items[this.selectedIndex]) {
            this.closeModal();
            return;
          }

          this.selected = { ...res.items[this.selectedIndex] } || ({} as LanguageTextDto);
        }
      });
  }

  edit(data: LanguageTextDto, index: number) {
    this.selectedIndex = index % this.list.maxResultCount;

    this.selected = { ...data };
    this.openModal();
  }

  save(next?: boolean) {
    if (this.modalBusy) return;
    this.modalBusy = true;

    const { resourceName, cultureName, name, value } = this.selected;

    this.service
      .update(resourceName, cultureName, name, value)
      .pipe(
        finalize(() => {
          setTimeout(() => {
            this.modalBusy = false;
          }, 200);

          if (!next) {
            this.closeModal();
          }
        }),
      )
      .subscribe(() => {
        if (next) {
          const { maxResultCount } = this.list;
          if (
            this.selectedIndex + 1 === this.data.totalCount % maxResultCount &&
            this.list.page * 10 + maxResultCount >= this.data.totalCount
          ) {
            this.closeModal();
            return;
          }

          if ((this.selectedIndex + 1) % maxResultCount === 0) {
            this.selectedIndex = 0;
            this.list.page = this.list.page + 1;
            this.renderer.removeClass(
              document.getElementById('LanguageTextToEdit_TargetCultureValue'),
              'ng-dirty',
            );
          } else {
            this.selectedIndex += 1;
            this.selected = { ...this.data.items[this.selectedIndex] } || ({} as LanguageTextDto);
            this.renderer.removeClass(
              document.getElementById('LanguageTextToEdit_TargetCultureValue'),
              'ng-dirty',
            );
          }
        }

        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.list.getWithoutPageReset();
      });
  }

  restore() {
    const { resourceName, cultureName, name } = this.selected;

    this.service.restoreToDefault(resourceName, cultureName, name).subscribe(() => {
      this.closeModal();
      this.list.get();
    });
  }
}
