import { TestBed } from '@angular/core/testing';

import { CreateFolderModalService } from './create-folder-modal.service';

describe('CreateFolderModalService', () => {
  let service: CreateFolderModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({ teardown: { destroyAfterEach: false } });
    service = TestBed.inject(CreateFolderModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
