import { Injectable } from '@angular/core';
import { skip, filter } from 'rxjs/operators';
import { merge } from 'rxjs';
import { InternalStore } from '@abp/ng.core';

export interface UpdateStreamState {
  content: number;
  createdDirectory: string;
  renamedDirectory: { id: string; name: string };
  movedDirectory: {
    id: string;
    newParentId: string;
    oldParentId: string;
  };
  deletedDirectory: string;
  currentDirectory: string;
}

@Injectable()
export class UpdateStreamService {
  private store = new InternalStore<Partial<UpdateStreamState>>({});
  readonly currentDirectory$ = this.sliceState(
    (state) => state.currentDirectory
  );
  get currentDirectory() {
    return this.store.state.currentDirectory;
  }

  readonly directoryRename$ = this.sliceState(
    (state) => state.renamedDirectory
  );
  readonly directoryDelete$ = this.sliceState(
    (state) => state.deletedDirectory
  );

  // this should be sliceUpdate, because if the user creates multiple folder within the same directory,
  // sliceState won't trigger the subscriber again since the directory has not changed.
  readonly directoryCreate$ = this.store.sliceUpdate(
    (state) => state.createdDirectory
  );
  readonly directoryMove$ = this.sliceState((state) => state.movedDirectory);

  readonly contentRefresh$ = merge(
    this.sliceState((state) => state.content),
    this.currentDirectory$,
    this.directoryCreate$.pipe(filter((id) => id === this.currentDirectory)),
    this.directoryMove$.pipe(
      filter(
        ({ oldParentId, newParentId }) =>
          oldParentId === this.currentDirectory ||
          newParentId === this.currentDirectory
      )
    )
  );

  patchStore = (state: Partial<UpdateStreamState>) => this.store.patch(state);

  refreshContent = () => this.store.patch({ content: new Date().getTime() });

  private sliceState<Slice>(fn: (state: UpdateStreamState) => Slice) {
    // skip initial empty values
    return this.store.sliceState(fn).pipe(skip(1));
  }
}
