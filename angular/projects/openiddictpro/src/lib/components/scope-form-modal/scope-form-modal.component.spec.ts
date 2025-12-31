import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScopeFormModalComponent } from './scope-form-modal.component';

describe('ScopeFormModalComponent', () => {
  let component: ScopeFormModalComponent;
  let fixture: ComponentFixture<ScopeFormModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ScopeFormModalComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScopeFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
