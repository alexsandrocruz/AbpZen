import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { MoveFileModalComponent } from './move-file-modal.component';

describe('MoveFileModalComponent', () => {
  let component: MoveFileModalComponent;
  let fixture: ComponentFixture<MoveFileModalComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [MoveFileModalComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveFileModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
