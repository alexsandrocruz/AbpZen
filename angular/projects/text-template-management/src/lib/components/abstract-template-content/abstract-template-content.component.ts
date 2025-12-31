import { ToasterService } from '@abp/ng.theme.shared';
import { Component, inject, Injector, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import {
  TemplateContentService,
  TextTemplateContentDto,
} from '@volo/abp.ng.text-template-management/proxy';
import { finalize, switchMap, tap } from 'rxjs/operators';

// Not an abstract class on purpose. Do not change!
@Component({
  template: '',
})
export class AbstractTemplateContentComponent implements OnInit {
  protected fb: UntypedFormBuilder = inject(UntypedFormBuilder);
  protected templateContentService: TemplateContentService = inject(TemplateContentService);
  protected route: ActivatedRoute = inject(ActivatedRoute);
  protected toasterService: ToasterService = inject(ToasterService);

  form: UntypedFormGroup;
  templateContent = {} as TextTemplateContentDto;
  busy: boolean;
  selectedCultureName: string;

  ngOnInit() {
    this.form = this.fb.group({ content: ['', [Validators.required]] });
    this.getData().subscribe();
  }

  getData() {
    const templateName = this.route.snapshot.params.name;
    return this.templateContentService
      .get({ templateName, cultureName: this.selectedCultureName })
      .pipe(
        tap(templateContent => {
          this.templateContent = templateContent;
          this.form.get('content').setValue(this.templateContent.content);
        }),
      );
  }

  save(callback?: () => unknown) {
    if (this.form.invalid) return;
    this.busy = true;

    const { content } = this.form.value;
    this.templateContentService
      .update({
        templateName: this.templateContent.name,
        cultureName: this.selectedCultureName,
        content,
      })
      .pipe(finalize(() => (this.busy = false)))
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
        if (callback) callback();
      });
  }

  restoreToDefault() {
    this.busy = true;

    this.templateContentService
      .restoreToDefault({
        templateName: this.templateContent.name,
        cultureName: this.selectedCultureName,
      })
      .pipe(
        switchMap(() => this.getData()),
        finalize(() => (this.busy = false)),
      )
      .subscribe(() => {
        this.toasterService.success('AbpUi::SavedSuccessfully');
      });
  }
}
