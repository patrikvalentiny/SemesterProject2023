import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeSkeletonComponent} from './home-skeleton/home-skeleton.component';
import {RouterOutlet} from "@angular/router";
import {HomeViewComponent} from './home-view/home-view.component';
import {WeightControlsModule} from '../pages/weight-controls/weight-controls.module';
import {PagesModule} from "../pages/pages.module";
import {ChartsModule} from "../charts/charts.module";
import {StatCardsModule} from "../pages/stat-cards/stat-cards.module";
import {CalculatedLossCardComponent} from "../pages/stat-cards/calculated-loss-card/calculated-loss-card.component";


@NgModule({
  declarations: [
    HomeSkeletonComponent,
    HomeViewComponent,
  ],
  exports: [
    HomeSkeletonComponent,
    HomeViewComponent,
  ],
    imports: [
        CommonModule,
        RouterOutlet,
        WeightControlsModule,
        PagesModule,
        ChartsModule,
        StatCardsModule,
        CalculatedLossCardComponent,
    ]
})
export class HomeModule {
}
