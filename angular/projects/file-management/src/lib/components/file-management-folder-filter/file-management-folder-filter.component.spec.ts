import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FileManagementFolderFilterComponent } from './file-management-folder-filter.component';

describe('FileManagementFolderFilterComponent', () => {
  let component: FileManagementFolderFilterComponent;
  let fixture: ComponentFixture<FileManagementFolderFilterComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [FileManagementFolderFilterComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FileManagementFolderFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
