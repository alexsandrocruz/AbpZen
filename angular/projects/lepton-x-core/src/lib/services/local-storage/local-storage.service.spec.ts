import { TestBed } from '@angular/core/testing';

import { LpxLocalStorageService } from './local-storage.service';

describe('LocalStorageService', () => {
  let service: LpxLocalStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LpxLocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
