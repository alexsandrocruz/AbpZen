import { TestBed } from '@angular/core/testing';

import { FileManagementService } from './file-management.service';

describe('FileManagementService', () => {
  let service: FileManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({ teardown: { destroyAfterEach: false } });
    service = TestBed.inject(FileManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
