import { TestBed } from '@angular/core/testing';

import { MoveFileModalService } from './move-file-modal.service';

describe('MoveFileModalService', () => {
  let service: MoveFileModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({ teardown: { destroyAfterEach: false } });
    service = TestBed.inject(MoveFileModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
