import {Component, inject, OnInit, ViewChild} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  ApexAnnotations,
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels, ApexMarkers,
  ApexStroke,
  ApexTheme,
  ApexTitleSubtitle, ApexTooltip,
  ApexXAxis,
  ApexYAxis,
  ChartComponent,
  NgApexchartsModule
} from "ng-apexcharts";
import {WeightService} from "../../services/weight.service";
import {UserDetailsService} from "../../services/user-details.service";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  theme: ApexTheme;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis[];
  title: ApexTitleSubtitle;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  annotations: ApexAnnotations;
  colors: string[];
  tooltip: ApexTooltip;
  markers: ApexMarkers;
};

@Component({
  selector: 'app-bmi-line-chart',
  standalone: true,
    imports: [CommonModule, NgApexchartsModule],
  templateUrl: './bmi-line-chart.component.html',
  styleUrl: './bmi-line-chart.component.css'
})
export class BmiLineChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly userService: UserDetailsService = inject(UserDetailsService);

  constructor() {
    this.chartOptions = {
        markers: {
            size: 2,
            hover: {
            size: 6
            }
        },
      colors:['#dca54c'],
      series: [
        {
          name: "Weight",
          data: [0]
        }
      ],
      chart: {
        id: "bmi",
        height: 300,
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
          seriesName: "BMI",
          opposite: false,
          title: {
            text: "BMI"
          },
        }

      ],
      dataLabels: {
        enabled: false
      },
      theme: {
        mode: "dark",
        palette: "palette10"
      },
      title: {
        text: "Your BMI history"
      },
      stroke: {
        show: true,
        curve: "smooth",
      },
      annotations: {
        yaxis: [{
          yAxisIndex: 0,
          y: 29.99,
          y2: 25,
          borderColor: "#000",
          fillColor: "#e2d562",
          opacity: 0.2,
          label: {
            borderColor: "#333",
            style: {
              fontSize: "10px",
              color: "#333",
              background: "#e2d562"
            },
            text: "Overweight"
          }
        },
          {
            yAxisIndex: 0,
            y: 24.99,
            y2: 18.5,
            borderColor: "#000",
            fillColor: "#87d039",
            opacity: 0.2,
            label: {
              borderColor: "#333",
              style: {
                fontSize: "10px",
                color: "#333",
                background: "#87d039"
              },
              text: "Normal weight"
            }
          },
          {
            yAxisIndex: 0,
            y: 18.49,
            y2: 0,
            borderColor: "#000",
            fillColor: "#66c6ff",
            opacity: 0.2,
            label: {
              borderColor: "#333",
              style: {
                fontSize: "10px",
                color: "#333",
                background: "#66c6ff"
              },
              text: "Underweight"
            }
          }]
      },
      tooltip:{
        shared: true,
        y:{
          formatter(val: number, opts?: any): string {
            return val + "BMI (" + (val - opts.w.globals.series[opts.seriesIndex][opts.dataPointIndex - 1 ] ).toFixed(2)+ ")";

          }
        },
      }
    };
  }

  async ngOnInit() {
    await this.userService.getProfile();
    const height = this.userService.user!.height / 100;
    const targetWeight = this.userService.user!.targetWeight;
    const targetWeightBmi = targetWeight / (height * height);
    const targetDate = this.userService.user!.targetDate;

    const bmi = await this.weightService.getBmi() ?? [];
    const bmiNums = bmi!.map(w => w.bmi);
    // range of dates from first in weight to target date
    const startDate = new Date(this.weightService.weights[0].date);
    const endDate = new Date(targetDate);

    const maxBmi = Math.max(...bmiNums) + 1;
    let minBmi = Math.min(...bmiNums) - 1;
    minBmi = minBmi < targetWeightBmi ? minBmi : targetWeightBmi - 1;
    this.chartOptions.yaxis![0].max = maxBmi;
    this.chartOptions.yaxis![0].min = minBmi;


    const seriesData = bmi.map(bmi => ({
      x: new Date(bmi.date).getTime(),
      y: bmi.bmi
    }));



    this.chartOptions.series = [
      {
        name: "BMI",
        data: seriesData
      },
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

    this.chartOptions.annotations!.yaxis!.push(
      {
      yAxisIndex: 0,
      y: 30,
      y2: maxBmi,
      borderColor: "#000",
      fillColor: "#ff6f6f",
      opacity: 0.2,
      label: {
        borderColor: "#333",
        style: {
          fontSize: "10px",
          color: "#333",
          background: "#ff6f6f"
        },
        text: "Obese"
      }
    },
      {
        yAxisIndex:0,
        y: targetWeightBmi,
        y2: targetWeightBmi + 0.1,
        borderColor: "#000",
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
          text: "Target BMI"
        }
      });

  }
}
