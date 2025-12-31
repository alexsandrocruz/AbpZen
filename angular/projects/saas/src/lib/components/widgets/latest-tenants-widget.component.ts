import { PermissionService } from '@abp/ng.core';
import { Component, Input } from '@angular/core';
import { GetTenantsInput, SaasTenantDto, TenantService } from '@volo/abp.ng.saas/proxy';

@Component({
  selector: 'abp-latest-tenants-widget',
  templateUrl: './latest-tenants-widget.component.html',
})
export class LatestTenantsWidgetComponent {
  data: SaasTenantDto[];

  @Input()
  minHeight = 136;

  trackByFn = (_, item) => item.id;

  constructor(private service: TenantService, private permissionService: PermissionService) {}

  draw = () => {
    if (!this.permissionService.getGrantedPolicy('Saas.Tenants')) {
      return;
    }

    this.service
      .getList({
        getEditionNames: true,
        maxResultCount: 6,
        skipCount: 0,
        sorting: 'CreationTime desc',
      } as GetTenantsInput)
      .subscribe(res => (this.data = res.items));
  };
}
