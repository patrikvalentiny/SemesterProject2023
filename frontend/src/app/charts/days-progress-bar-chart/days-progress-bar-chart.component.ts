import { CommonModule } from '@angular/common';
import {Component, inject, OnInit, ViewChild} from "@angular/core";

import {
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexChart, ChartComponent, NgApexchartsModule, ApexTheme, ApexDataLabels, ApexTooltip, ApexLegend
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
  legend:ApexLegend;
};
@Component({
  selector: 'app-days-progress-bar-chart',
  standalone: true,
    imports: [CommonModule, NgApexchartsModule],
  templateUrl: './days-progress-bar-chart.component.html',
  styleUrl: './days-progress-bar-chart.component.css'
})
export class DaysProgressBarChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  private readonly statService: StatisticsService = inject(StatisticsService);

  constructor() {
    this.chartOptions = {
      legend:{
        show: true,
        position: 'right',
      },
      colors: ["#dca54c", "#152747"],
      series: [0],
      tooltip: {
        enabled: true,
        y:{
          formatter(val: number, opts?: any): string {
            return val + " Days";
          }
        }
      },
      chart: {
        width: 350,
        height: 250,
        type: "donut",
        background: 'transparent',
      },
      plotOptions: {
        pie: {
          dataLabels: {
            offset: 0,
          },
          donut: {
            size: "70%",
            labels:{
              total:{
                label: 'Total days',
                show: true,
                showAlways: true,
                fontSize: '11px',
                fontFamily: 'Helvetica, Arial, sans-serif',
                fontWeight: 400,
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
      labels: ["Days in", "Days to go"],
      theme:{mode: 'dark'},
      dataLabels: {
        enabled: true,

        style: {
          fontSize: '18px',
          fontFamily: 'Helvetica, Arial, sans-serif',
          fontWeight: 400,
          colors: ['#fff']
        },
        formatter: function (val: number, opts?: any): string {
          return opts.w.config.series[opts.seriesIndex] + " Days";
        }
      }

    };
  }

  async ngOnInit(){
    const daysToGo = await this.statService.getDaysToGo();
    const daysIn = await this.statService.getDayIn();
    this.chartOptions.series = [daysIn, daysToGo];
  }
}
