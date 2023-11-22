import {Component, inject, OnInit, ViewChild} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexTitleSubtitle,
  ApexXAxis,
  ChartComponent,
  NgApexchartsModule
} from "ng-apexcharts";
import {WeightService} from "../../weight-controls/weight.service";

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
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
          name: "My-series",
          data: [0]
        }
      ],
      chart: {
        height: 350,
        type: "line"
      },
      title: {
        text: "Your weight history"
      }
    };
  }

  async ngOnInit() {
    await this.weightService.getWeights();
    const weights: number[] = this.weightService.weights.map(weight => weight.weight).reverse();
    const dates: string[] = this.weightService.weights.map(weight => weight.date.substring(0, 10)).reverse();
    this.chartOptions.series = [{
      data: weights
    }];
    this.chartOptions.xaxis = {
          categories: dates
        };

  }

}
