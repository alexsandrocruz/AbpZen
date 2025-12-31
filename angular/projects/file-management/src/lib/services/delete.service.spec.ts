import { TestBed } from '@angular/core/testing';

import { DeleteService } from './delete.service';

describe('DeleteService', () => {
  let service: DeleteService;

  beforeEach(() => {
    TestBed.configureTestingModule({ teardown: { destroyAfterEach: false } });
    service = TestBed.inject(DeleteService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
