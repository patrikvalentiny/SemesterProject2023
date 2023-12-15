import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {WeightDto} from "../../../dtos/weight-dto";
import {StatisticsService} from "../../../services/statistics.service";

@Component({
  selector: 'app-current-stats-card',
  templateUrl: './current-stats-card.component.html',
  styleUrl: './current-stats-card.component.css'
})
export class CurrentStatsCardComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  statService: StatisticsService = inject(StatisticsService);
  currentWeight: WeightDto | undefined;
  lowestWeight: WeightDto | undefined;

  constructor() {
  }

  async ngOnInit() {
    try {
      this.currentWeight = await this.weightService.getLatestWeight();
      this.lowestWeight = await this.statService.getLowestWeight();
    } catch (e){
      //caught by interceptor
      return;
    }

  }
}
