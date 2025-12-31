import { Component, Inject, OnInit } from '@angular/core';
import { PROP_DATA_STREAM } from '@abp/ng.components/extensible';
import { Observable, switchMap } from 'rxjs';
import { AbpWindowService, EnvironmentService, SubscriptionService } from '@abp/ng.core';
import { GdprRequestDto, GdprRequestService } from '@volo/abp.ng.gdpr/proxy';
import { take } from 'rxjs/operators';

@Component({
  selector: 'abp-gdpr-action-column',
  templateUrl: './gdpr-action-column.component.html',
  providers: [SubscriptionService],
})
export class GdprActionColumnComponent implements OnInit {
  isReadyToDownload = false;

  get apiUrl() {
    return this.environment.getApiUrl('Gdpr');
  }
  constructor(
    @Inject(PROP_DATA_STREAM) public value$: Observable<GdprRequestDto>,
    protected environment: EnvironmentService,
    private gdprRequestService: GdprRequestService,
    private subscription: SubscriptionService,
    private abpWindowService: AbpWindowService,
  ) {}

  ngOnInit() {
    this.subscription.addOne(this.value$, list => {
      this.isReadyToDownload = this.checkIsReadyToDownload(list.readyTime);
    });
  }

  getUserData() {
    this.value$.pipe(take(1)).subscribe(gdprRequest => {
      if (this.checkIsReadyToDownload(gdprRequest.readyTime)) {
        this.gdprRequestService
          .getDownloadToken(gdprRequest.id)
          .pipe(switchMap(res => this.gdprRequestService.getUserData(gdprRequest.id, res.token)))
          .subscribe(file => {
            this.abpWindowService.downloadBlob(file, 'PersonalData');
          });
      }
    });
  }

  checkIsReadyToDownload(readyTime: string) {
    return new Date(readyTime) < new Date();
  }
}
