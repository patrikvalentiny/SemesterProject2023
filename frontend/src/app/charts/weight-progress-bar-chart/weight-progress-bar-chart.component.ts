import { CommonModule } from '@angular/common';
import {Component, inject, OnInit, ViewChild} from "@angular/core";

import {
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexChart, ChartComponent, NgApexchartsModule, ApexTheme
} from "ng-apexcharts";
import {StatisticsService} from "../../services/statistics.service";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
  colors: string[];
  theme: ApexTheme;
};
@Component({
  selector: 'app-weight-progress-bar-chart',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule],
  templateUrl: './weight-progress-bar-chart.component.html',
  styleUrl: './weight-progress-bar-chart.component.css'
})
export class WeightProgressBarChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  private readonly statService: StatisticsService = inject(StatisticsService);

  constructor() {
    this.chartOptions = {
      colors: ["#dca54c"],
      series: [0],
      chart: {
        height: 350,
        type: "radialBar",
        background: 'transparent'
      },
      plotOptions: {
        radialBar: {
          hollow: {
            size: "80%"
          }
        }
      },
      labels: ["% of goal"],
      theme:{mode: 'dark'}
    };
  }

  async ngOnInit(){
    this.chartOptions.series = [await this.statService.getPercentageOfGoal()];
  }
}
