import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {UserDetailsService} from "../../../services/user-details.service";
import {WeightDto} from "../../../dtos/weight-dto";
import {Bmi} from "../../../dtos/bmi";
import {StatisticsService} from "../../../services/statistics.service";

@Component({
  selector: 'app-current-stats-card',
  templateUrl: './current-stats-card.component.html',
  styleUrl: './current-stats-card.component.css'
})
export class CurrentStatsCardComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  statService: StatisticsService = inject(StatisticsService);
  currentWeight : WeightDto | undefined;
  currentLoss: number = 0;
  predictedTargetWeight: WeightDto | undefined;
  constructor() {
  }

  async ngOnInit() {
    this.currentWeight = await this.weightService.getLatestWeight();
    this.currentLoss = await this.statService.getCurrentTotalLoss();
  }
}
