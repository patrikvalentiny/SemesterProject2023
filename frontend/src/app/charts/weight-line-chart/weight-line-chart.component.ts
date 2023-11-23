import {Component, inject, OnInit, ViewChild} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  ApexAxisChartSeries,
  ApexChart, ApexDataLabels, ApexStroke, ApexTheme,
  ApexTitleSubtitle,
  ApexXAxis,
  ChartComponent,
  NgApexchartsModule
} from "ng-apexcharts";
import {WeightService} from "../../weight-controls/weight.service";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  theme:ApexTheme;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
  stroke:ApexStroke;
  dataLabels:ApexDataLabels;
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

  constructor() {
    this.chartOptions = {
      series: [
        {
          name: "Weight",
          data: [0]
        }
      ],
      chart: {
        height: 350,
        type: "area",
        background:"rgba(0,0,0,0)",
        toolbar: {
          show: true,
          offsetX: 0,
          offsetY: 0,
          tools: {
            download: true,
            selection: false,
            zoom: true,
            zoomin: false,
            zoomout: false,
            pan: false,
          }
        },
        zoom:{
          enabled:true,
          type:"x"

        }
      },
      dataLabels:{
        enabled:false
      },
      theme:{
        mode:"dark",
        palette:"palette10"
      },
      title: {
        text: "Your weight history"
      },
      stroke:{
        show:true,
        curve:"smooth",
        lineCap:"round"
      }
    };
  }

  async ngOnInit() {
    await this.weightService.getWeights();
    const weights: number[] = this.weightService.weights.map(weight => weight.weight).reverse();
    const dates: string[] = this.weightService.weights.map(weight => new Date(weight.date).toLocaleString()).reverse();
    this.chartOptions.series = [{
      name:"Weight",
      data: weights
    }];
    this.chartOptions.xaxis = {
          categories: dates
        };

  }

}
