import { TestBed } from '@angular/core/testing';

import { MenuFilterService } from './menu-filter.service';

describe('MenuFilterService', () => {
  let service: MenuFilterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MenuFilterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
