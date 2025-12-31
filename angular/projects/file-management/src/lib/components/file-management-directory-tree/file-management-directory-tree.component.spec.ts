import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FileManagementDirectoryTreeComponent } from './file-management-directory-tree.component';

describe('FileManagementDirectoryTreeComponent', () => {
  let component: FileManagementDirectoryTreeComponent;
  let fixture: ComponentFixture<FileManagementDirectoryTreeComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [FileManagementDirectoryTreeComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FileManagementDirectoryTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
