import {Component, inject, OnInit, ViewChild} from '@angular/core';
import { ChartComponent, NgApexchartsModule } from "ng-apexcharts";
import {WeightService} from "../../services/weight.service";
import {UserDetailsService} from "../../services/user-details.service";
import {StatisticsService} from "../../services/statistics.service";
import {HotToastService} from "@ngneat/hot-toast";
import {AxisChartOptions, defaultAxisChartOptions} from "../chart-helper";


@Component({
    selector: 'app-trend-line-chart',
    templateUrl: './trend-line-chart.component.html',
    styleUrl: './trend-line-chart.component.css',
    standalone: true,
    imports: [NgApexchartsModule]
})
export class TrendLineChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<AxisChartOptions>;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly userService: UserDetailsService = inject(UserDetailsService);
  private readonly statService: StatisticsService = inject(StatisticsService);
  private readonly toast = inject(HotToastService);

  constructor() {
    this.chartOptions = {
      markers: {
        size: 1,
        hover: {
          size: 3
        }
      },
      series: [
        {
          name: "Weight",
          data: [0]
        }
      ],
      chart: {
        id: "trend",
        height: 200,
        type: "area",
        group: "weight",
        background: "rgba(0,0,0,0)",
        toolbar: {
          show: true,
          offsetX: 0,
          offsetY: 0,
          tools: {
            download: true,
            selection: true,
            zoom: true,
            zoomin: true,
            zoomout: true,
            pan: true,
          }
        },
        zoom: {
          type: "x",
          enabled: true,
          autoScaleYaxis: true

        }
      },
      yaxis: [
        {
          seriesName: "Weight",
          title: {text: "Weight (kg)"},
        }
      ],
      title: {
        text: "Weight Prediction"
      },
      annotations: {
        xaxis: [
          {
            x: new Date().setHours(0, 0, 0, 0),
            strokeDashArray: 0,
            borderColor: "#333",
            label: {
              borderColor: "#333",
              style: {
                color: "#fff",
                background: "#00000000"
              },
              text: "Today"
            }
          }
        ],
        yaxis: []
      },
      tooltip: {
        shared: true,
      },
    };
  }

  async ngOnInit() {
    try {
      await this.weightService.getWeights();
      await this.userService.getProfile();

      const targetWeight = this.userService.user!.targetWeight;


      const weights = await this.statService.getTrend();


      const weightNums = weights.map(w => w.weight);

      const startDate = new Date(weights[0].date);
      const endDate = new Date(weights.at(-1)!.date);
      const maxWeight = Math.max(...weightNums) + 2;
      let minWeight = Math.min(...weightNums) - 2;
      minWeight = minWeight < targetWeight ? minWeight - 2 : targetWeight - 2;
      this.chartOptions.yaxis![0].max = maxWeight;
      this.chartOptions.yaxis![0].min = minWeight;
      this.chartOptions.yaxis![0].tickAmount = 3;

      const seriesData = weights.map(weight => ({
        x: new Date(weight.date).getTime(),
        y: weight.weight
      }));


      this.chartOptions.series = [
        {
          name: "Weight",
          data: seriesData
        },
      ];
      this.chartOptions.xaxis = {
        type: "datetime",
        max: endDate.getTime(),
        min: startDate.getTime(),
        tickAmount: 10,
        labels: {
          format: "dd/MM/yy",
          datetimeUTC: false,
        },
        // categories: dates
      };

      this.chartOptions.annotations!.yaxis! = [
        {
          yAxisIndex: 0,
          y: targetWeight,
          y2: targetWeight + 0.1,
          borderColor: "#00dbe3",
          fillColor: "#00dbe3",
          opacity: 1,
          label: {
            position: "left",
            textAnchor: "start",
            offsetX: 10,
            borderColor: "#333",
            style: {
              fontSize: "10px",
              color: "#333",
              background: "#00dbe3"
            },
            text: "Target weight"
          }
        }
      ];
    } catch (e) {
      this.toast.error("Error loading chart")
      return;
    }
  }

  protected readonly defaultAxisChartOptions = defaultAxisChartOptions;
}
