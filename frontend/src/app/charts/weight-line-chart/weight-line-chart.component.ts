import {Component, inject, OnInit, ViewChild} from '@angular/core';
import { ChartComponent, NgApexchartsModule } from "ng-apexcharts";
import {WeightService} from "../../services/weight.service";
import {UserDetailsService} from "../../services/user-details.service";
import {HotToastService} from "@ngneat/hot-toast";
import {AxisChartOptions, defaultAxisChartOptions} from "../chart-helper";

@Component({
    selector: 'app-weight-line-chart',
    templateUrl: './weight-line-chart.component.html',
    styleUrl: './weight-line-chart.component.css',
    standalone: true,
    imports: [NgApexchartsModule]
})
export class WeightLineChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<AxisChartOptions>;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly userService: UserDetailsService = inject(UserDetailsService);
  private readonly toast = inject(HotToastService);

  constructor() {

    this.chartOptions = {
      markers: {
        size: 2,
        hover: {
          size: 6
        }
      },
      series: [
        {
          name: "Weight",
          data: [0]
        },
        {
          name: "Body Fat",
          data: [0]
        }
      ],
      chart: {
        id: "weight",
        height: 400,
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
          },
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
        text: "Your weight history"
      },
      annotations: {
        yaxis: [],
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
        ]
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
      }
    };
  }

  async ngOnInit() {
    try {
      await this.weightService.getWeights();
      await this.userService.getProfile();
      const targetWeight = this.userService.user!.targetWeight;
      const targetDate = new Date(this.userService.user!.targetDate);

      const weights = this.weightService.weights;
      const weightNums = weights.map(w => w.weight);

      const startDate = new Date(this.weightService.weights[0].date);
      const today = new Date();
      const endDate = targetDate > today ? targetDate : today;
      const maxWeight = Math.max(...weightNums) + 2;
      let minWeight = Math.min(...weightNums) - 2;
      minWeight = minWeight < targetWeight ? minWeight - 2 : targetWeight - 2;
      this.chartOptions.yaxis![0].max = maxWeight;
      this.chartOptions.yaxis![0].min = 0;
      this.chartOptions.yaxis![0].tickAmount = Math.ceil((maxWeight - minWeight) / 10) + 2;

      const seriesData = weights.map(weight => ({
        x: new Date(weight.date).getTime(),
        y: weight.weight
      }));
      const bodyFatData = weights.filter(weight => weight.bodyFatPercentage).map(weight => ({
        x: new Date(weight.date).getTime(),
        y: (weight.weight * (weight.bodyFatPercentage! ?? 0) / 100).toFixed(1)
      }));
      const skeletalMuscleData = weights.filter(weight => weight.skeletalMuscleWeight).map(weight => ({
        x: new Date(weight.date).getTime(),
        y: weight.skeletalMuscleWeight! ?? 0
      }));


      this.chartOptions.series = [
        {
          name: "Weight",
          data: seriesData
        },
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
        labels: {
          format: "dd/MM/yy",
          datetimeUTC: false,
        },
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
      this.toast.error("Could not load weight chart data");
    }
  }

  protected readonly defaultAxisChartOptions = defaultAxisChartOptions;
}
