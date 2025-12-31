import { ComponentRef, Injector } from '@angular/core';
import { ContentProjectionService, PROJECTION_STRATEGY } from '@abp/ng.core';
import { MyLinkUsersModalComponent, AuthorityDelegationComponent } from '../components';
import { AbpAuthorityDelegationService } from '../services';

export function openMyLinkUsersFactory(injector: Injector) {
  const contentProjectionService = injector.get(ContentProjectionService);
  let componentRef: ComponentRef<MyLinkUsersModalComponent>;

  return () => {
    if (componentRef) {
      componentRef.instance.isModalOpen = true;
      return;
    }

    componentRef = contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(MyLinkUsersModalComponent),
    );
  };
}

export function openAuthorityDelegationFactory(injector: Injector) {
  const contentProjectionService = injector.get(ContentProjectionService);
  const delegationService = injector.get(AbpAuthorityDelegationService);

  return () => {
    if (delegationService.modalRef) {
      delegationService.modalRef.instance.isModalOpen = true;
      return;
    }

    delegationService.modalRef = contentProjectionService.projectContent(
      PROJECTION_STRATEGY.AppendComponentToBody(AuthorityDelegationComponent),
    );
  };
}
