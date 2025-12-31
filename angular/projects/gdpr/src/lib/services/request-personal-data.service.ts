import { Injectable } from '@angular/core';
import { InternalStore, ListService } from '@abp/ng.core';
import { GdprRequestService } from '@volo/abp.ng.gdpr/proxy';

@Injectable()
export class RequestPersonalDataService {
  private store = new InternalStore<boolean>(false);

  btnDisabled$ = this.store.sliceState(s => !s);

  constructor(private list: ListService, private gdprRequestService: GdprRequestService) {}

  requestPersonalDate() {
    this.gdprRequestService.prepareUserData().subscribe(() => {
      this.list.get();
      this.checkRequestAllowed();
    });
  }

  checkRequestAllowed() {
    this.gdprRequestService.isNewRequestAllowed().subscribe(res => {
      this.store.patch(res);
    });
  }
}
