import {Component, inject, OnInit, ViewChild} from "@angular/core";

import {
  ApexChart,
  ApexDataLabels,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexTheme,
  ApexTooltip,
  ChartComponent
} from "ng-apexcharts";
import {StatisticsService} from "../../services/statistics.service";
import {HotToastService} from "@ngneat/hot-toast";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
  colors: string[];
  theme: ApexTheme;
  dataLabels: ApexDataLabels;
  tooltip: ApexTooltip;
  legend: ApexLegend;
};

@Component({
  selector: 'app-weight-progress-bar-chart',
  templateUrl: './weight-progress-bar-chart.component.html',
  styleUrl: './weight-progress-bar-chart.component.css'
})
export class WeightProgressBarChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  private readonly statService: StatisticsService = inject(StatisticsService);
  private readonly toast = inject(HotToastService);

  constructor() {
    this.chartOptions = {
      legend: {
        show: true,
        position: 'bottom',
      },
      colors: ["#dca54c", "#152747"],
      series: [0],
      tooltip: {
        enabled: true,
        y: {
          formatter(val: number): string {
            return val + "kg";
          }
        }
      },
      chart: {
        height: 250,
        type: "donut",
        background: 'transparent',
      },
      plotOptions: {
        pie: {
          donut: {

            size: "70%",
            labels: {
              total: {
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
                  return (seriesTotals[0] / sum * 100).toFixed(1) + "%";
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
              value: {
                formatter(val: string): string {
                  return val;
                }
              }

            }
          }
        }
      },
      labels: ["Total Loss", "Weight to go"],
      theme: {mode: 'dark'},
      dataLabels: {
        enabled: true,
        style: {
          fontSize: '18px',
          fontFamily: 'Helvetica, Arial, sans-serif',
          fontWeight: 400,
        },
        formatter: function (val: number, opts: any) {
          return opts.w.config.series[opts.seriesIndex] + "kg";
        }
      }

    };
  }

  async ngOnInit() {
    try {
      const totalLoss = await this.statService.getCurrentTotalLoss();
      const weightToGo = await this.statService.getWeightToGo();
      this.chartOptions.series = [totalLoss, weightToGo];
    } catch (e) {
      this.toast.error("Could not load weight progress");
    }

  }
}
