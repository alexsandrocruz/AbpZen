import { ConfigStateService, LanguageInfo } from '@abp/ng.core';
import { Component, inject, Injector, OnInit } from '@angular/core';
import { TextTemplateContentDto } from '@volo/abp.ng.text-template-management/proxy';
import { AbstractTemplateContentComponent } from '../abstract-template-content/abstract-template-content.component';

@Component({
  selector: 'abp-template-contents',
  templateUrl: 'template-contents.component.html',
})
export class TemplateContentsComponent extends AbstractTemplateContentComponent implements OnInit {
  private readonly configStateService = inject(ConfigStateService);

  languages: LanguageInfo[] = [];
  currentCultureName: string;
  referenceTemplateContent = { content: '' } as TextTemplateContentDto;

  ngOnInit() {
    this.languages = this.configStateService.getDeep('localization.languages');
    this.selectedCultureName = this.languages[0].cultureName;
    this.referenceTemplateContent.cultureName = this.configStateService.getDeep(
      'localization.currentCulture.cultureName',
    );
    super.ngOnInit();
    this.getReferenceTemplateContent();
  }

  getReferenceTemplateContent() {
    this.templateContentService
      .get({
        templateName: this.route.snapshot.params.name,
        cultureName: this.referenceTemplateContent.cultureName,
      })
      .subscribe(res => {
        this.referenceTemplateContent = res;
      });
  }

  onChangeSelectedCultureName() {
    this.getData().subscribe();
  }
}
