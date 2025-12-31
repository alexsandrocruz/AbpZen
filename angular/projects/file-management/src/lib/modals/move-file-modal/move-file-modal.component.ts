import { ListService } from '@abp/ng.core';
import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DirectoryContentDto } from '@volo/abp.ng.file-management/proxy';
import { map } from 'rxjs/operators';
import { FolderInfo } from '../../models/common-types';
import { BaseModalComponent } from '../base-modal.component';
import { MoveFileModalService } from './move-file-modal.service';

@Component({
  selector: 'abp-move-file-modal',
  templateUrl: './move-file-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MoveFileModalService, ListService],
})
export class MoveFileModalComponent extends BaseModalComponent implements OnChanges {
  @Input() fileToMove: DirectoryContentDto;
  @Input() oldParentId: string;

  directoryContent$ = this.service.directoryContent$.pipe(
    map(content => content.filter(item => item.id !== this.fileToMove.id)),
  );

  constructor(public readonly service: MoveFileModalService) {
    super();
  }

  ngOnChanges({ visible }: SimpleChanges) {
    if (visible.currentValue) {
      this.service.reset();
    }
  }

  onBreadcrumbClick(folder: FolderInfo) {
    this.service.goTo(folder);
  }

  saveAction() {
    if (this.fileToMove.isDirectory) {
      return this.service.moveFolder(this.fileToMove, this.oldParentId);
    } else {
      return this.service.move(this.fileToMove);
    }
  }
}
