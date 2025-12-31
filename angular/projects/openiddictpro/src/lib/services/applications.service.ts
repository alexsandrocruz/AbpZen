import { InternalStore } from '@abp/ng.core';
import { ApplicationsServiceState } from '../models';
import { computed, signal } from '@angular/core';

export class ApplicationsService {
  private state = new InternalStore({
    isModalVisible: false,
  } as ApplicationsServiceState);

  #isModalVisible = signal(false);
  isModalVisible = computed(() => this.#isModalVisible());
  #isTokenLifetimeModalVisible = signal(false);
  isTokenLifetimeModalVisible = computed(() => this.#isTokenLifetimeModalVisible());

  openModal() {
    this.setModalState(true);
  }

  setModalState(value: boolean) {
    this.#isModalVisible.set(value);
  }

  setTokenLifetimeModalState(value: boolean) {
    this.#isTokenLifetimeModalVisible.set(value);
  }
}
