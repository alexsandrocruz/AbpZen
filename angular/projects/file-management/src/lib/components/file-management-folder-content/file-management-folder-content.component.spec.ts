import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FileManagementFolderContentComponent } from './file-management-folder-content.component';

describe('FileManagementFolderContentComponent', () => {
  let component: FileManagementFolderContentComponent;
  let fixture: ComponentFixture<FileManagementFolderContentComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [FileManagementFolderContentComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FileManagementFolderContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
