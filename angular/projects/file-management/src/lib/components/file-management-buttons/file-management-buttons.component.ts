import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Inject,
  Renderer2,
  ViewEncapsulation,
  inject,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Observable } from 'rxjs';
import { map, switchMap, take, tap } from 'rxjs/operators';
import { UppyOptions } from '@uppy/core';
import { LocalizationService, PermissionService, SubscriptionService } from '@abp/ng.core';
import { eFileManagementPolicyNames } from '@volo/abp.ng.file-management/config';
import { UPPY_OPTIONS } from '@volo/abp.ng.file-management/common';
import { FolderInfo } from '../../models';
import { UploadService } from '../../services/upload.service';
import { NavigatorService } from '../../services/navigator.service';
import { ROOT_NODE } from '../../services/directory-tree.service';

const TITLE_CLASS_LIST = ['upload-folder-title', 'text-center', 'mb-1', 'mt-1', 'order-first'];

@Component({
  selector: 'abp-file-management-buttons',
  templateUrl: './file-management-buttons.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: [
    '../../../../../../node_modules/@uppy/core/dist/style.min.css',
    '../../../../../../node_modules/@uppy/dashboard/dist/style.min.css',
  ],
  encapsulation: ViewEncapsulation.None,
})
export class FileManagementButtonsComponent implements AfterViewInit {
  protected readonly subscriptionService = inject(SubscriptionService);
  protected readonly modalTitle = 'FileManagement::UploadFileModalTitle';

  folderCreateModal = false;
  fileCreatePermission = eFileManagementPolicyNames.FileDescriptorCreate;
  directoryCreatePermission = eFileManagementPolicyNames.DirectoryDescriptorCreate;

  buttonId = 'upload-files-btn';
  uppyWrapperParentComponent: Element;
  uploadFolderTitle: Element;
  currentFolder: FolderInfo = ROOT_NODE;

  protected get titleElement(): Element {
    return this.document.querySelector('h3.upload-folder-title');
  }

  protected initTitle(): Observable<string> {
    return this.navigatorService.currentFolder$.asObservable().pipe(
      map(({ name }) => name),
      tap(name => this.createUploadFolderTitle(name)),
      map(name => this.localizationService.instant(this.modalTitle, name)),
      tap(folderName => this.renderer.setProperty(this.uploadFolderTitle, 'innerText', folderName)),
    );
  }

  //TODO: @masumulu28 Find a way much better
  protected createUploadFolderTitle(folderName: string): void {
    this.uppyWrapperParentComponent = this.document.querySelector('div.uppy-Dashboard-innerWrap');

    if (!this.titleElement) {
      const title = this.renderer.createElement('h3');
      TITLE_CLASS_LIST.map(className => this.renderer.addClass(title, className));

      folderName = this.localizationService.instant(this.modalTitle, folderName);
      this.renderer.setProperty(title, 'innerText', folderName);
      this.renderer.appendChild(this.uppyWrapperParentComponent, title);
    }
    this.uploadFolderTitle = this.titleElement;
  }

  constructor(
    private uploadService: UploadService,
    private permissionService: PermissionService,
    private navigatorService: NavigatorService,
    private localizationService: LocalizationService,
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef,
    @Inject(DOCUMENT) private readonly document: Document,
    @Inject(UPPY_OPTIONS) private readonly uppyOptions: UppyOptions<any>,
  ) {}

  ngAfterViewInit(): void {
    const init$ = this.permissionService.getGrantedPolicy$(this.fileCreatePermission).pipe(
      take(1),
      tap(() =>
        this.uploadService.initUppy(
          this.elementRef.nativeElement.querySelector(`#${this.buttonId}`),
          this.uppyOptions,
        ),
      ),
      switchMap(() => this.initTitle()),
    );

    this.subscriptionService.addOne(init$);
  }
}
