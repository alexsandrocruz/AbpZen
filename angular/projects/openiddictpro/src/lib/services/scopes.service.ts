import { InternalStore } from '@abp/ng.core';
import { ScopesServiceState } from '../models';

export class ScopesService {
  private state = new InternalStore({ isModalVisible: false } as ScopesServiceState);
  isModalVisible$ = this.state.sliceState(x => x.isModalVisible);

  openModal() {
    this.setModalState(true);
  }
  setModalState(value: boolean) {
    this.state.patch({ isModalVisible: value });
  }
}
