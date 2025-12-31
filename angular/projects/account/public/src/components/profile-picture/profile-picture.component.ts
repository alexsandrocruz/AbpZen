import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  Inject,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren,
  ViewEncapsulation,
} from '@angular/core';
import { BehaviorSubject, EMPTY, Subject } from 'rxjs';
import { catchError, filter, finalize, switchMap, take, tap } from 'rxjs/operators';
import Cropper from 'cropperjs/dist/cropper.esm.js';
import { ConfigStateService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  DEFAULT_PROFILE_ICON,
  eProfilePictureType,
  ProfilePictureImage,
  ProfilePictureService,
  PROFILE_PICTURE,
} from '@volo/abp.commercial.ng.ui/config';

@Component({
  selector: 'abp-profile-picture',
  templateUrl: 'profile-picture.component.html',
  styleUrls: [
    '../../../../../../node_modules/cropperjs/dist/cropper.css',
    'profile-picture.component.scss',
  ],
  // ViewEncapsulation.None is important. Do not change! Otherwise cropper.css will not work.
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilePictureComponent implements OnInit {
  protected readonly cdRef = inject(ChangeDetectorRef);
  protected readonly confirmation = inject(ConfirmationService);
  protected readonly profilePictureService = inject(ProfilePictureService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly profilePicture$ = inject(PROFILE_PICTURE);

  @ViewChild('uploadFile', { read: ElementRef }) uploadFileRef: ElementRef<HTMLInputElement>;
  @ViewChild('selectedImgRef', { read: ElementRef }) selectedImgRef: ElementRef<HTMLImageElement>;
  @ViewChildren('preview', { read: ElementRef }) selectedImagePreviews: QueryList<
    ElementRef<HTMLImageElement>
  >;

  profileLoaded: boolean;
  inProgress: boolean;
  profilePictureType = eProfilePictureType.None;
  selectedImage: any;
  cropper: Cropper;

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id');
  }

  private toBase64(file) {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = error => reject(error);
    });
  }

  ngOnInit() {
    this.getProfilePhoto();
  }

  getProfilePhoto() {
    this.profilePictureService
      .getProfilePicture(this.currentUserId)
      .pipe(
        finalize(() => {
          this.profileLoaded = true;
          this.cdRef.markForCheck();
        }),
      )
      .subscribe({
        next: res => {
          this.profilePictureType = res.type;
          const pP = res.source || `data:image/png;base64,${res.fileContent}`;
          
          if (this.profilePicture$.value.source !== pP) {
            this.profilePicture$.next({ type: 'image', source: pP });
          }
        },
        error: () => this.profilePicture$.next(DEFAULT_PROFILE_ICON),
      });
  }

  submit() {
    let localization = '';
    switch (this.profilePictureType) {
      case eProfilePictureType.Gravatar:
        localization = 'UseGravatarConfirm';
        break;
      case eProfilePictureType.Image:
        localization = 'PPUploadConfirm';
        break;
      default:
        localization = 'NoProfilePictureConfirm';
        break;
    }

    this.confirmation
      .warn(`AbpAccount::${localization}`, 'AbpAccount::AreYouSure')
      .pipe(
        filter(res => res === Confirmation.Status.confirm),
        tap(() => (this.inProgress = true)),
        switchMap(() => {
          if (this.profilePictureType === eProfilePictureType.Image) {
            const subject = new Subject();

            this.cropper.getCroppedCanvas().toBlob(blob => {
              this.profilePictureService
                .setProfilePicture({
                  type: this.profilePictureType,
                  imageContent: blob,
                })
                .pipe(
                  finalize(() => subject.complete()),
                  catchError(error => {
                    subject.error(error);
                    return EMPTY;
                  }),
                )
                .subscribe(result => subject.next(result));
            });

            return subject.asObservable();
          }

          return this.profilePictureService.setProfilePicture({
            type: this.profilePictureType,
          });
        }),
        take(1),
        finalize(() => {
          this.inProgress = false;
          const scrollContainer = document.getElementsByClassName('lpx-scroll-container')[0];
          if (scrollContainer) {
            scrollContainer.scrollTo({ top: 0, behavior: 'instant' });
          }
        }),
      )
      .subscribe(() => {
        this.getProfilePhoto();
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.selectedImage = null;
        
        if (this.uploadFileRef) {
          this.uploadFileRef.nativeElement.value = null;
        }
      });
  }

  async onSelectImage(file: File) {
    this.selectedImage = await this.toBase64(file);
    this.cdRef.detectChanges();

    if (this.cropper) {
      this.cropper.destroy();
    }

    const previewSizes = [250, 150, 75];

    const setImgUrls = () => {
      this.selectedImagePreviews.forEach((el, i) => {
        const width = previewSizes[i];
        el.nativeElement.src = this.cropper.getCroppedCanvas({ width, height: width }).toDataURL();
      });
    };

    this.cropper = new Cropper(this.selectedImgRef.nativeElement, {
      aspectRatio: 1,
      viewMode: 1,
      cropend: () => setImgUrls(),
      ready: () => setImgUrls(),
    });
  }
}
