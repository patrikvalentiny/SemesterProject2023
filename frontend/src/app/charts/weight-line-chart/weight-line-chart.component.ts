import {Component, inject, OnInit, ViewChild} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  ApexAnnotations,
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
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
import {StatisticsService} from "../../services/statistics.service";

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
};

@Component({
  selector: 'app-weight-line-chart',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule],
  templateUrl: './weight-line-chart.component.html',
  styleUrl: './weight-line-chart.component.css'
})
export class WeightLineChartComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly userService: UserDetailsService = inject(UserDetailsService);

  constructor() {

    this.chartOptions = {
      colors: ["#dca54c", "#ff0000", "#00ff00", "#0000ff"],
      series: [
        {
          name: "Weight",
          data: [0]
        }
      ],
      chart: {
        height: 350,
        type: "area",
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
          enabled: true,
          type: "xy"

        }
      },
      yaxis: [
        {
          seriesName: "Weight",
          title: {text: "Weight (kg)"},
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
        text: "Your weight history"
      },
      stroke: {
        show: true,
        curve: "smooth",
      },
      annotations: {
        yaxis: []
      },
      tooltip:{
        shared: true,
      }
    };
  }

  async ngOnInit() {
    await this.weightService.getWeights();
    await this.userService.getProfile();
    const targetWeight = this.userService.user!.targetWeight;
    const targetDate = this.userService.user!.targetDate;

    const weights = this.weightService.weights;
    const weightNums = weights.map(w => w.weight);

    const startDate = new Date(this.weightService.weights[0].date);
    const endDate = new Date(targetDate);
    const maxWeight = Math.max(...weightNums) + 2;
    let minWeight = Math.min(...weightNums) - 2;
    minWeight = minWeight < targetWeight ? minWeight - 2 : targetWeight - 2;
    this.chartOptions.yaxis![0].max = maxWeight;
    this.chartOptions.yaxis![0].min = minWeight;

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

    this.chartOptions.annotations!.yaxis! =  [
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

  }

}
