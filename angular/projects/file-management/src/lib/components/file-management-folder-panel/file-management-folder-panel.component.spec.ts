import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FileManagementFolderPanelComponent } from './file-management-folder-panel.component';

describe('FileManagementFolderPanelComponent', () => {
  let component: FileManagementFolderPanelComponent;
  let fixture: ComponentFixture<FileManagementFolderPanelComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [FileManagementFolderPanelComponent],
        teardown: { destroyAfterEach: false },
      }).compileComponents();
    }),
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(FileManagementFolderPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
