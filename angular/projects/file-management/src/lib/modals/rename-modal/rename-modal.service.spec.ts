import { TestBed } from '@angular/core/testing';

import { RenameModalService } from './rename-modal.service';

describe('RenameModalService', () => {
  let service: RenameModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({ teardown: { destroyAfterEach: false } });
    service = TestBed.inject(RenameModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
