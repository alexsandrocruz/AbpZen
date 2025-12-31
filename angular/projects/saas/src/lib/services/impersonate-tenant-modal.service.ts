import { Injectable } from '@angular/core';
import { InternalStore } from '@abp/ng.core';
import { ImpersonateTenantModalState } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ImpersonateTenantModalService {
  private readonly store = new InternalStore({
    tenantId: null,
    isVisible: undefined,
  } as ImpersonateTenantModalState);

  isVisible$ = this.store.sliceState(x => x.isVisible);

  get tenantId() {
    return this.store.state.tenantId;
  }

  private setTenantId(val: string | null) {
    this.store.patch({ tenantId: val });
  }

  public setValue(val: boolean) {
    this.store.patch({ isVisible: val });
  }

  public hide() {
    this.setValue(false);
    this.setTenantId(null);
  }

  public show(tenantId: string | null) {
    this.setValue(true);
    this.setTenantId(tenantId);
  }
}
