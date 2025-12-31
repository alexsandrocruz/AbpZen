import { Inject, Injectable } from '@angular/core';
import { DocumentDirHandlerService } from '@abp/ng.theme.shared';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class DocumentDirService {
  constructor(
    private documentDirHandler: DocumentDirHandlerService,
    @Inject(DOCUMENT) private document: Document
  ) {}

  listenDir() {
    this.documentDirHandler.dir$.subscribe((dir) => {
      this.document.documentElement.dir = dir;
    });
  }
}
