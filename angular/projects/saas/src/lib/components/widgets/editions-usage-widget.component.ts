import { ChartComponent, getRandomBackgroundColor } from '@abp/ng.components/chart.js';
import { PermissionService, SubscriptionService } from '@abp/ng.core';
import { Component, Input, Output, ViewChild } from '@angular/core';
import { EditionService } from '@volo/abp.ng.saas/proxy';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'abp-editions-usage-widget',
  templateUrl: './editions-usage-widget.component.html',
  providers: [SubscriptionService],
})
export class EditionsUsageWidgetComponent {
  data: Record<string, number> = {};

  @Output() initialized = new BehaviorSubject(this);

  @Input()
  width = 273;

  @Input()
  height = 136;

  @ViewChild(ChartComponent) chart: ChartComponent;

  chartData: any = {};

  draw = () => {
    if (!this.permissionService.getGrantedPolicy('Saas.Editions')) {
      return;
    }

    this.service.getUsageStatistics().subscribe(res => {
      this.data = res.data;
      this.setChartData();
    });
  };

  constructor(private permissionService: PermissionService, private service: EditionService) {}

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
            backgroundColor: getRandomBackgroundColor(Object.keys(this.data).length),
          },
        ],
      };

      this.chart.refresh();
    }, 0);
  }
}
