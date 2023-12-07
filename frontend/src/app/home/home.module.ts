import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeSkeletonComponent} from './home-skeleton/home-skeleton.component';
import {RouterOutlet} from "@angular/router";
import {HomeViewComponent} from './home-view/home-view.component';
import {WeightControlsModule} from '../pages/weight-controls/weight-controls.module';
import {WeightLineChartComponent} from "../charts/weight-line-chart/weight-line-chart.component";
import {CurrentStatsCardComponent} from "../pages/stat-cards/current-stats-card/current-stats-card.component";
import {DaysStatsCardComponent} from "../pages/stat-cards/days-stats-card/days-stats-card.component";
import {BmiLineChartComponent} from "../charts/bmi-line-chart/bmi-line-chart.component";
import {BmiStatsCardComponent} from "../pages/stat-cards/bmi-stats-card/bmi-stats-card.component";
import {TrendLineChartComponent} from "../charts/trend-line-chart/trend-line-chart.component";
import {PredictedStatsCardComponent} from "../pages/stat-cards/predicted-stats-card/predicted-stats-card.component";
import {WeightProgressBarChartComponent} from "../charts/weight-progress-bar-chart/weight-progress-bar-chart.component";


@NgModule({
    declarations: [
        HomeSkeletonComponent,
        HomeViewComponent,
        CurrentStatsCardComponent,
        DaysStatsCardComponent
    ],
    exports: [
        HomeSkeletonComponent,
        HomeViewComponent,
        CurrentStatsCardComponent,
        DaysStatsCardComponent
    ],
    imports: [
        CommonModule,
        RouterOutlet,
        WeightControlsModule,
        WeightLineChartComponent,
        BmiLineChartComponent,
        BmiStatsCardComponent,
        TrendLineChartComponent,
        PredictedStatsCardComponent,
        WeightProgressBarChartComponent,
    ]
})
export class HomeModule {
}
