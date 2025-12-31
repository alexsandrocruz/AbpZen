import { SubscriptionService } from '@abp/ng.core';
import { DropEvent, TreeNode, TreeComponent } from '@abp/ng.components/tree';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewChild,
} from '@angular/core';
import { eFileManagementPolicyNames } from '@volo/abp.ng.file-management/config';
import {
  DirectoryContentDto,
  DirectoryDescriptorInfoDto,
} from '@volo/abp.ng.file-management/proxy';
import { NzFormatEmitEvent, NzTreeNode } from 'ng-zorro-antd/tree';
import { FolderInfo } from '../../models/common-types';
import { DeleteService } from '../../services/delete.service';
import { DirectoryTreeService, ROOT_ID } from '../../services/directory-tree.service';
import { MoveService } from '../../services/move.service';
import { NavigatorService } from '../../services/navigator.service';
import { UpdateStreamService } from '../../services';

type RequiredDirectoryDescriptorInfoDto = Required<DirectoryDescriptorInfoDto>;

@Component({
  selector: 'abp-file-management-directory-tree',
  templateUrl: './file-management-directory-tree.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [SubscriptionService],
})
export class FileManagementDirectoryTreeComponent implements OnInit {
  directories: TreeNode<RequiredDirectoryDescriptorInfoDto>[];

  createPolicy = eFileManagementPolicyNames.DirectoryDescriptorCreate;
  updatePolicy = eFileManagementPolicyNames.DirectoryDescriptorUpdate;
  deletePolicy = eFileManagementPolicyNames.DirectoryDescriptorDelete;
  contextMenuPolicy = `${this.createPolicy} || ${this.updatePolicy} || ${this.deletePolicy}`;

  createModalVisible = false;
  createModalParentId = '';

  renameModalVisible = false;
  contentToRename: DirectoryContentDto;

  moveModalVisible = false;
  folderToMove: DirectoryContentDto;
  parentOfFolderToMove: string;

  rootId = ROOT_ID;
  selectedNode: any;
  @ViewChild(TreeComponent) tree: TreeComponent;

  updateDirectories = (directories: TreeNode<RequiredDirectoryDescriptorInfoDto>[]) => {
    this.directories = directories.map(this.markAsParentHasChildren);
    this.cdr.markForCheck();
  };

  updateSelectedNode = (directory: string) => {
    const id = directory || ROOT_ID;
    this.tree.setSelectedNode({ id });
  };

  markAsParentHasChildren = (node: TreeNode<RequiredDirectoryDescriptorInfoDto>) => {
    return {
      ...node,
      isLeaf: !node.entity.hasChildren && !node.children.length,
      children: node?.children?.map(child => this.markAsParentHasChildren(child)),
    };
  };

  constructor(
    public service: DirectoryTreeService,
    private navigator: NavigatorService,
    private move: MoveService,
    private cdr: ChangeDetectorRef,
    private deleteService: DeleteService,
    private subscription: SubscriptionService,
    private updateStream: UpdateStreamService,
  ) {}

  ngOnInit(): void {
    this.subscription.addOne(this.service.directoryTree$, this.updateDirectories);
    this.subscription.addOne(this.updateStream.currentDirectory$, this.updateSelectedNode);
  }

  onDrop({ dragNode, node }: DropEvent) {
    this.move
      .moveTo(
        this.nzNodeToFolderInfo(dragNode),
        this.nzNodeToFolderInfo(node),
        dragNode.parentNode.origin.id,
      )
      .subscribe();
  }

  onNodeClick(node) {
    if (node.isRoot) {
      this.onRootClick();
    } else {
      this.navigator.goToFolder(node);
    }
  }

  onRootClick() {
    this.navigator.goToRoot();
  }

  onCreate(folder: DirectoryContentDto) {
    this.createModalVisible = true;
    this.createModalParentId = folder.id;
  }

  onRename(folder: DirectoryContentDto) {
    this.renameModalVisible = true;
    this.contentToRename = { ...folder, isDirectory: true };
  }

  onDelete(folder: DirectoryContentDto) {
    this.deleteService.deleteFolder(folder).subscribe();
  }

  onMove(folder: NzTreeNode) {
    this.folderToMove = { ...folder.origin.entity, isDirectory: true };
    this.parentOfFolderToMove = folder.parentNode.origin.id;
    this.moveModalVisible = true;
  }

  onContentRenamed(folder: DirectoryContentDto) {
    this.service.refreshNode({ newParentId: folder.id });
  }

  afterFolderMoved() {
    delete this.folderToMove;
  }

  private nzNodeToFolderInfo(node: NzTreeNode): FolderInfo {
    return {
      id: node.origin.id,
      name: node.origin.title,
    };
  }

  onExpandedKeysChange(event: NzFormatEmitEvent) {
    if (event.node.isExpanded && event.node.key !== ROOT_ID && !event.node.children.length) {
      this.service.updateDirectories(event.node.key);
    }
  }
}
