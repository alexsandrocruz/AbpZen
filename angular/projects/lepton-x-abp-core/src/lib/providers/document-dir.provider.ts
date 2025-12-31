import { APP_INITIALIZER } from '@angular/core';
import { DocumentDirService } from '../services/document-dir.service';

export const DOCUMENT_DIR_PROVIDER = {
  provide: APP_INITIALIZER,
  multi: true,
  deps: [DocumentDirService],
  useFactory: listenDirectionChangeFromAbp
}

export function listenDirectionChangeFromAbp(documentDirService: DocumentDirService) {
  return () => {
    documentDirService.listenDir();
  }
}
