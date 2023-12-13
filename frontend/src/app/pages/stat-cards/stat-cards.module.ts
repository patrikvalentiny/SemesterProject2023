import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PredictedStatsCardComponent} from "./predicted-stats-card/predicted-stats-card.component";
import {DaysStatsCardComponent} from "./days-stats-card/days-stats-card.component";
import {CurrentStatsCardComponent} from "./current-stats-card/current-stats-card.component";
import {BmiStatsCardComponent} from "./bmi-stats-card/bmi-stats-card.component";
import {AverageLossCardComponent} from "./average-loss-card/average-loss-card.component";



@NgModule({
  declarations: [
    PredictedStatsCardComponent,
    DaysStatsCardComponent,
    CurrentStatsCardComponent,
    BmiStatsCardComponent,
    AverageLossCardComponent
  ],
  exports: [
    AverageLossCardComponent,
    CurrentStatsCardComponent,
    BmiStatsCardComponent,
    PredictedStatsCardComponent
  ],
  imports: [
    CommonModule
  ]
})
export class StatCardsModule { }
