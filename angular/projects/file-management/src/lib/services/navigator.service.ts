import { Injectable, OnDestroy } from '@angular/core';
import { InternalStore } from '@abp/ng.core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { FolderInfo } from '../models/common-types';
import { DirectoryTreeService, mapRootIdToEmpty, ROOT_NODE } from './directory-tree.service';
import { UpdateStreamService } from './update-stream.service';

@Injectable()
export class NavigatorService implements OnDestroy {
  private navigatedFolders: FolderInfo[] = [ROOT_NODE];
  private store = new InternalStore<FolderInfo[]>(this.navigatedFolders);

  navigatedFolderPath$ = this.store.sliceState(
    state => state,
    (s1, s2) => s1 === s2,
  );

  subscription = new Subscription();
  currentFolder$ = new BehaviorSubject<FolderInfo>(ROOT_NODE);

  constructor(private directory: DirectoryTreeService, private updateStream: UpdateStreamService) {
    this.setupListeners();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  goToFolder(folder: FolderInfo) {
    this.currentFolder$.next(folder || ROOT_NODE);
    this.updateCurrentFolderPath(folder);
  }

  goToRoot() {
    this.goToFolder(null);
  }

  goUpFolder() {
    if (this.navigatedFolders.length === 0) {
      // already at the root, do nothing.
      return;
    } else if (this.navigatedFolders.length === 1) {
      this.goToRoot();
    } else {
      const folder = this.navigatedFolders[this.navigatedFolders.length - 2];
      this.goToFolder(folder);
    }
  }

  getCurrentFolder() {
    return this.navigatedFolders.length && this.navigatedFolders[this.navigatedFolders.length - 1];
  }

  getCurrentFolderId() {
    return mapRootIdToEmpty(this.getCurrentFolder()?.id);
  }

  private updateCurrentFolderPath(folder: FolderInfo) {
    this.next([...this.directory.findFullPathOf(folder)]);
  }

  private next(newValue: FolderInfo[]) {
    this.navigatedFolders = newValue;
    this.store.patch(this.navigatedFolders);
    this.currentFolder$.next(this.getCurrentFolder());
    this.updateStream.patchStore({
      currentDirectory: this.getCurrentFolderId(),
    });
  }

  private updateFolderName = (updatedFolder: { id: string; name: string }) => {
    const newNavigatedFolders = [...this.navigatedFolders];
    newNavigatedFolders
      .filter(folder => folder.id === updatedFolder.id)
      .forEach(folder => (folder.name = updatedFolder.name));
    this.next(newNavigatedFolders);
  };

  private removeDeletedFolder = (id: string) => {
    const index = this.findIndexOf(id);
    if (index > -1) {
      this.next(this.navigatedFolders.slice(0, index));
    }
  };

  private updateMovedFolder = ({ id, newParentId }: { id: string; newParentId: string }) => {
    const selfIndex = this.findIndexOf(id);
    // this means that moved folder is being shown in the breadcrumb and needs to be dealt with
    if (selfIndex > -1) {
      const parentIndex = this.findIndexOf(newParentId);
      let newPaths = [];
      // if the new parent is found, this means that this folder is moved upwards within its own absolute path.
      // then we can simply move to the its new parent.
      if (parentIndex > -1) {
        newPaths = this.navigatedFolders.slice(0, parentIndex + 1);
      } else {
        // if the new parent is not found, it could mean either that it is somewhere else in the tree or it has not been visited at all.
        // It is hard to find the new absolute path, so just return to the root.
        newPaths = [this.directory.getRootNode()];
      }
      this.next(newPaths);
    }
  };

  private setupListeners() {
    this.subscription.add(this.updateStream.directoryRename$.subscribe(this.updateFolderName));
    this.subscription.add(this.updateStream.directoryDelete$.subscribe(this.removeDeletedFolder));
    this.subscription.add(this.updateStream.directoryMove$.subscribe(this.updateMovedFolder));
  }

  private findIndexOf(folderId: string) {
    return this.navigatedFolders.findIndex(folder => folder.id === folderId);
  }
}
