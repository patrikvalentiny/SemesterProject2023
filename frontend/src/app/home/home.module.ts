import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeSkeletonComponent} from './home-skeleton/home-skeleton.component';
import {RouterOutlet} from "@angular/router";
import {HomeViewComponent} from './home-view/home-view.component';
import {WeightControlsModule} from '../pages/weight-controls/weight-controls.module';
import {WeightLineChartComponent} from "../charts/weight-line-chart/weight-line-chart.component";
import {CurrentStatsCardComponent} from "./current-stats-card/current-stats-card.component";
import {DaysStatsCardComponent} from "./days-stats-card/days-stats-card.component";


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
    ]
})
export class HomeModule {
}
