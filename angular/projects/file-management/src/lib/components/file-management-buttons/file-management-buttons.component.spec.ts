import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FileManagementButtonsComponent } from './file-management-buttons.component';

describe('FileManagementButtonsComponent', () => {
  let component: FileManagementButtonsComponent;
  let fixture: ComponentFixture<FileManagementButtonsComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [FileManagementButtonsComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FileManagementButtonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
