import { BaseNode, TreeAdapter, TreeNode } from '@abp/ng.components/tree';
import { InternalStore } from '@abp/ng.core';
import { Injectable, OnDestroy } from '@angular/core';
import {
  DirectoryDescriptorInfoDto,
  DirectoryDescriptorService,
} from '@volo/abp.ng.file-management/proxy';
import { forkJoin, merge, Subscription } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { FolderInfo } from '../models/common-types';
import { UpdateStreamService } from './update-stream.service';

export type TreeType = BaseNode & DirectoryDescriptorInfoDto;
export const ROOT_ID = 'ROOT_ID';
export const ROOT_NODE = {
  id: ROOT_ID,
  parentId: null,
  name: 'All Files',
  isRoot: true,
  hasChildren: true,
} as any;

export function mapRootIdToEmpty(id: string) {
  return id === ROOT_ID ? null : id;
}

@Injectable()
export class DirectoryTreeService implements OnDestroy {
  private treeHolder: TreeAdapter<TreeType>;

  private store = new InternalStore<TreeNode<TreeType>[]>([]);
  directoryTree$ = this.store.sliceState(state => state);

  subscription = new Subscription();

  extendedKeys = [];

  parents = [];

  constructor(
    private directoryService: DirectoryDescriptorService,
    private updateStream: UpdateStreamService,
  ) {
    this.setupListener();
  }

  updateDirectories = (currentFolderId: string = null) => {
    this.collapseFolder(currentFolderId);
    this.fetchDirectories(mapRootIdToEmpty(currentFolderId)).subscribe(items => {
      this.updateTree(currentFolderId, items);
    });
  };

  refreshNode = ({ newParentId }) => {
    this.parents = [];
    this.findAncestors(this.treeHolder.getTree(), newParentId);
    const reversedParentsArray = this.parents.reverse();

    if (!this.parents.length) {
      this.updateDirectories();
      return;
    }

    const observables = {};
    this.store.reset();
    this.extendedKeys = [];

    for (const [index, id] of reversedParentsArray.entries()) {
      observables[index] = this.directoryService
        .getList(mapRootIdToEmpty(id))
        .pipe(map(result => this.bindInitialListToRoot(result.items)));
    }

    forkJoin(observables).subscribe(val => {
      for (const [index, id] of reversedParentsArray.entries()) {
        this.updateTree(id, val[index]);
      }
    });
  };

  findAncestors(tree: TreeNode<TreeType>[], targetId: string = 'ROOT_ID') {
    tree.forEach(node => {
      if (node.id !== targetId && node.children) {
        this.findAncestors(node.children, targetId);
        return;
      }
      this.parents.push(node.id);
      this.findAncestors(this.treeHolder.getTree(), node.parentId);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  setupListener() {
    this.subscription.add(
      merge(
        this.updateStream.directoryRename$.pipe(map(folder => this.getParentOf(folder.id))),
        this.updateStream.directoryDelete$.pipe(map(id => this.getParentOf(id))),
      ).subscribe(this.updateDirectories),
    );

    this.subscription.add(
      merge(
        this.updateStream.directoryMove$,
        this.updateStream.directoryDelete$.pipe(map(val => ({ newParentId: val }))),
      ).subscribe(this.refreshNode),
    );
  }

  findFullPathOf(folder: FolderInfo): FolderInfo[] {
    let currentId = folder?.id;
    const fullList = this.treeHolder.getList();
    const retVal = [];

    while (currentId) {
      const node = fullList.find(i => i.id === currentId);
      if (node && node.id !== ROOT_ID) {
        retVal.push(node);
      }
      currentId = node?.parentId;
    }

    return retVal
      .concat(ROOT_NODE)
      .map(item => ({ id: item.id, name: item.name }))
      .reverse();
  }

  getParentOf(id: string) {
    const node = this.treeHolder.getList().find(item => item.id === id);
    return node?.parentId || ROOT_ID;
  }

  getRootNode() {
    return ROOT_NODE;
  }

  collapseFolder(id: string) {
    const index = this.extendedKeys.indexOf(id);
    if (index > -1) {
      this.extendedKeys.splice(index, 1);
    }
  }

  private updateTree(currentFolderId: string, items: DirectoryDescriptorInfoDto[]) {
    if (items) {
      if (this.treeHolder && currentFolderId) {
        this.onTreeExistAndNavigatedToNode(currentFolderId, items as TreeType[]);
      } else {
        this.onTreeNotExistOrNavigatedToRoot(items as TreeType[]);
      }
      this.updateStore();
    }
  }

  private updateStore() {
    this.store.patch(this.treeHolder.getTree() as any);
  }

  private onTreeExistAndNavigatedToNode(parentId: string, list: TreeType[]) {
    this.treeHolder.handleUpdate({ key: parentId, children: list });
    this.extendedKeys = [...this.extendedKeys, parentId];
  }

  private onTreeNotExistOrNavigatedToRoot(result: TreeType[]) {
    this.extendedKeys = [ROOT_ID];
    this.treeHolder = new TreeAdapter([ROOT_NODE, ...result]);
  }

  private fetchDirectories(id: string) {
    return this.directoryService
      .getList(id)
      .pipe(map(result => this.bindInitialListToRoot(result.items)));
  }

  private bindInitialListToRoot(list: DirectoryDescriptorInfoDto[]) {
    return list.map(l => ({ ...l, parentId: l.parentId || ROOT_ID }));
  }
}
