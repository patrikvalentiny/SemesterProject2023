import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";
import {WeightDto} from "../../../dtos/weight-dto";
import { DatePipe } from '@angular/common';

@Component({
    selector: 'app-predicted-stats-card',
    templateUrl: './predicted-stats-card.component.html',
    styleUrl: './predicted-stats-card.component.css',
    standalone: true,
    imports: [DatePipe]
})
export class PredictedStatsCardComponent implements OnInit {
  statService: StatisticsService = inject(StatisticsService);
  predictedTargetWeight: WeightDto | undefined;
  predictedTargetDate: Date | undefined;

  constructor() {
  }

  async ngOnInit() {
    try {
      this.predictedTargetWeight = await this.statService.getPredictedTargetWeight();
      this.predictedTargetDate = new Date(await this.statService.getPredictedTargetDate());
    } catch (e) {
      //caught by interceptor
      return;
    }

  }

}
