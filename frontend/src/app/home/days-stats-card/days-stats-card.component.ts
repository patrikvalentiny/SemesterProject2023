import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../services/weight.service";
import {StatisticsService} from "../../services/statistics.service";
import {WeightDto} from "../../dtos/weight-dto";

@Component({
  selector: 'app-days-stats-card',
  templateUrl: './days-stats-card.component.html',
  styleUrl: './days-stats-card.component.css'
})
export class DaysStatsCardComponent implements OnInit {
  statService: StatisticsService = inject(StatisticsService);
  daysToGo : number | undefined;
  dayIn: number | undefined;
  predictedTargetWeight: WeightDto | undefined;
  constructor() {
  }

  async ngOnInit() {
    this.daysToGo = await this.statService.getDaysToGo();
    this.dayIn = await this.statService.getDayIn();
    this.predictedTargetWeight = await this.statService.getPredictedTargetWeight();
  }
}
