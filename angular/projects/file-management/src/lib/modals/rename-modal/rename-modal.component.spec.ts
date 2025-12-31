import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RenameModalComponent } from './rename-modal.component';

describe('RenameModalComponent', () => {
  let component: RenameModalComponent;
  let fixture: ComponentFixture<RenameModalComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [RenameModalComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(RenameModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
