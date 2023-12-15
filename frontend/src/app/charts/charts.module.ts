import {NgModule} from '@angular/core';
import {CommonModule} from "@angular/common";
import {BmiLineChartComponent} from "./bmi-line-chart/bmi-line-chart.component";
import {NgApexchartsModule} from "ng-apexcharts";
import {DaysProgressBarChartComponent} from "./days-progress-bar-chart/days-progress-bar-chart.component";
import {TrendLineChartComponent} from "./trend-line-chart/trend-line-chart.component";
import {WeightLineChartComponent} from "./weight-line-chart/weight-line-chart.component";
import {WeightProgressBarChartComponent} from "./weight-progress-bar-chart/weight-progress-bar-chart.component";


@NgModule({
  declarations: [
    BmiLineChartComponent,
    DaysProgressBarChartComponent,
    TrendLineChartComponent,
    WeightLineChartComponent,
    WeightProgressBarChartComponent
  ],
  exports: [
    WeightProgressBarChartComponent,
    DaysProgressBarChartComponent,
    WeightLineChartComponent,
    BmiLineChartComponent,
    TrendLineChartComponent
  ],
  imports: [
    CommonModule,
    NgApexchartsModule,
  ]
})
export class ChartsModule {
}
