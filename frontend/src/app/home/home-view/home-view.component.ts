import {Component, OnInit} from '@angular/core';
import { TrendLineChartComponent } from '../../charts/trend-line-chart/trend-line-chart.component';
import { PredictedStatsCardComponent } from '../../pages/stat-cards/predicted-stats-card/predicted-stats-card.component';
import { BmiLineChartComponent } from '../../charts/bmi-line-chart/bmi-line-chart.component';
import { BmiStatsCardComponent } from '../../pages/stat-cards/bmi-stats-card/bmi-stats-card.component';
import { WeightLineChartComponent } from '../../charts/weight-line-chart/weight-line-chart.component';
import { CurrentStatsCardComponent } from '../../pages/stat-cards/current-stats-card/current-stats-card.component';
import { DaysProgressBarChartComponent } from '../../charts/days-progress-bar-chart/days-progress-bar-chart.component';
import { CalculatedLossCardComponent } from '../../pages/stat-cards/calculated-loss-card/calculated-loss-card.component';
import { AverageLossCardComponent } from '../../pages/stat-cards/average-loss-card/average-loss-card.component';
import { WeightProgressBarChartComponent } from '../../charts/weight-progress-bar-chart/weight-progress-bar-chart.component';

@Component({
    selector: 'app-home-view',
    host: { class: 'h-full' },
    templateUrl: './home-view.component.html',
    styleUrls: ['./home-view.component.css'],
    standalone: true,
    imports: [WeightProgressBarChartComponent, AverageLossCardComponent, CalculatedLossCardComponent, DaysProgressBarChartComponent, CurrentStatsCardComponent, WeightLineChartComponent, BmiStatsCardComponent, BmiLineChartComponent, PredictedStatsCardComponent, TrendLineChartComponent]
})
export class HomeViewComponent implements OnInit {

  constructor() {

  }

  async ngOnInit() {

  }
}
