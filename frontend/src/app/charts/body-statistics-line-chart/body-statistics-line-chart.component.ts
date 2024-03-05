import {Component, inject, OnInit, ViewChild} from '@angular/core';
import { ChartComponent, NgApexchartsModule } from "ng-apexcharts";
import {WeightService} from "../../services/weight.service";
import {UserDetailsService} from "../../services/user-details.service";
import {HotToastService} from "@ngneat/hot-toast";
import {AxisChartOptions, defaultAxisChartOptions} from "../chart-helper";


@Component({
    selector: 'app-body-statistics-line-chart',
    templateUrl: './body-statistics-line-chart.component.html',
    styleUrl: './body-statistics-line-chart.component.css',
    standalone: true,
    imports: [NgApexchartsModule]
})
export class BodyStatisticsLineChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<AxisChartOptions>;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly userService: UserDetailsService = inject(UserDetailsService);
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
        height: 250,
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
        text: "Body statistics"
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
        y: {
          formatter(val: number, opts?: any): string {
            if (opts.dataPointIndex === 0) {
              return val + " kg";
            }
            return val + " kg (" + (val - opts.w.globals.series[opts.seriesIndex][opts.dataPointIndex - 1]).toFixed(2) + ")";
          }
        }
      },
    };
  }

  async ngOnInit() {
    try {
      await this.weightService.getWeights();

      const weights = this.weightService.weights;
      const targetDate = new Date(this.userService.user!.targetDate);
      const startDate = new Date(this.weightService.weights[0].date);
      const today = new Date();
      const endDate = targetDate > today ? targetDate : today;
      const bodyFatData = weights.filter(
        weight => weight.bodyFatPercentage !== null
      ).map(weight => ({
        x: new Date(weight.date).getTime(),
        y: (weight.weight * (weight.bodyFatPercentage! ?? 0) / 100).toFixed(1)
      }));
      const skeletalMuscleData = weights
        .filter(
          weight => weight.skeletalMuscleWeight !== null
        ).map(weight => ({
        x: new Date(weight.date).getTime(),
        y: weight.skeletalMuscleWeight! ?? 0
      }));



      this.chartOptions.series = [
        {
          name: "Body Fat",
          data: bodyFatData
        },
        {
          name: "Skeletal Muscle",
          data: skeletalMuscleData
        }
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
    } catch (e) {
      this.toast.error("Error loading chart")
      return;
    }
  }

  protected readonly defaultAxisChartOptions = defaultAxisChartOptions;
}
