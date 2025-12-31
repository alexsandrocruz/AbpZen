import { ChartComponent } from '@abp/ng.components/chart.js';
import { PermissionService } from '@abp/ng.core';
import { Statistics } from '@abp/ng.theme.shared';
import { Component, Input, Output, ViewChild } from '@angular/core';
import { AuditLogsService } from '@volo/abp.ng.audit-logging/proxy';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'abp-error-rate-widget',
  template: `
    <div *abpPermission="'AuditLogging.AuditLogs'" class="abp-widget-wrapper">
      <div class="card">
        <div class="card-header">
          <h5 class="card-title">
            {{ 'AbpAuditLogging::ErrorRateInLogs' | abpLocalization }}
          </h5>
        </div>
        <div class="card-body">
          <div class="row">
            <abp-chart
              #chart
              class="w-100"
              type="pie"
              [data]="chartData"
              [width]="width"
              [height]="height"
            ></abp-chart>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ErrorRateWidgetComponent {
  data: Statistics.Data = {};

  @Output() initialized = new BehaviorSubject(this);

  @Input() width = 273;

  @Input() height = 136;

  @ViewChild(ChartComponent) chart: ChartComponent;

  chartData: any = {};

  draw = (filter: { startDate: string; endDate: string }) => {
    if (!this.permissionService.getGrantedPolicy('AuditLogging.AuditLogs')) {
      return;
    }

    this.service
      .getErrorRate({
        startDate: filter.startDate,
        endDate: filter.endDate,
      })
      .subscribe(res => {
        this.data = res.data;
        this.setChartData();
      });
  };

  constructor(
    protected permissionService: PermissionService,
    protected service: AuditLogsService,
  ) {}

  private setChartData() {
    if (!this.data || JSON.stringify(this.data) === '{}') {
      this.chartData = {};
      return;
    }
    const dataKeys = Object.keys(this.data);

    setTimeout(() => {
      this.chartData = {
        labels: dataKeys,
        datasets: [
          {
            data: dataKeys.map(key => this.data[key]),
            backgroundColor: ['#d76e6e', '#63ac44'],
          },
        ],
      };

      this.chart.refresh();
    }, 0);
  }
}
