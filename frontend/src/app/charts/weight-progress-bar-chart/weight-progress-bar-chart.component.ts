import { CommonModule } from '@angular/common';
import {Component, inject, OnInit, ViewChild} from "@angular/core";

import {
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexChart, ChartComponent, NgApexchartsModule, ApexTheme, ApexDataLabels, ApexTooltip
} from "ng-apexcharts";
import {StatisticsService} from "../../services/statistics.service";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
  colors: string[];
  theme: ApexTheme;
  dataLabels: ApexDataLabels;
  tooltip: ApexTooltip;
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
      colors: ["#dca54c", "#152747"],
      series: [0],
      tooltip: {
        enabled: true,
        y:{
          formatter(val: number, opts?: any): string {
            return val + "kg";
          }
        }
      },
      chart: {
        height: '100%',
        type: "donut",
        background: 'transparent',
      },
      plotOptions: {
        pie: {
          donut: {
            size: "70%",
            labels:{
              total:{
                show: true,
                showAlways: true,
                label: '% of goal',
                fontSize: '11px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
                formatter: function (w: any): string {
                  const seriesTotals = w.globals.seriesTotals
                  const sum = seriesTotals.reduce((a: number, b: number) => {
                    return a + b;
                  }, 0);
                  // check for division by 0
                  if (sum === 0) {
                    return "0%";
                  }
                  return Math.round(seriesTotals[0] / sum * 100) + "%";
                }
              },

              show: true,
              name: {
                show: true,
                fontSize: '10px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 600,
                color: '#373d3f',
              },
              value:{
                formatter(val: string): string {
                  return val;
                }
              }

            }
          }
        }
      },
      labels: ["Total Loss", "Weight to go"],
      theme:{mode: 'dark'},
      dataLabels: {
        enabled: true,
        formatter: function (val: number, opts: any) {
          return opts.w.config.series[opts.seriesIndex] + "kg";
        }
      }

    };
  }

  async ngOnInit(){
    const percentageOfGoal = await this.statService.getPercentageOfGoal();
    const totalLoss = await this.statService.getCurrentTotalLoss();
    const weightToGo = await this.statService.getWeightToGo();
    this.chartOptions.series = [totalLoss, weightToGo];
  }
}
