import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  template: `
    <app-host-dashboard *abpPermission="'LeptonXDemoApp.Dashboard.Host'"></app-host-dashboard>
    <app-tenant-dashboard *abpPermission="'LeptonXDemoApp.Dashboard.Host'"></app-tenant-dashboard>
  `,
})
export class DashboardComponent {}
