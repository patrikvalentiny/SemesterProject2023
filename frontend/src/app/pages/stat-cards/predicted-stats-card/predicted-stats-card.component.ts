import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";
import {WeightDto} from "../../../dtos/weight-dto";

@Component({
  selector: 'app-predicted-stats-card',
  templateUrl: './predicted-stats-card.component.html',
  styleUrl: './predicted-stats-card.component.css'
})
export class PredictedStatsCardComponent implements OnInit {
  statService: StatisticsService = inject(StatisticsService);
  predictedTargetWeight: WeightDto | undefined;
  predictedTargetDate: Date | undefined;
  constructor() {
  }

  async ngOnInit() {
    this.predictedTargetWeight = await this.statService.getPredictedTargetWeight();
    this.predictedTargetDate = new Date(await this.statService.getPredictedTargetDate());
  }

}
