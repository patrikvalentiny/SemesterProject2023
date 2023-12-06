import {Component, inject, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {WeightService} from "../../../services/weight.service";
import {StatisticsService} from "../../../services/statistics.service";
import {WeightDto} from "../../../dtos/weight-dto";
import {Bmi} from "../../../dtos/bmi";

@Component({
  selector: 'app-bmi-stats-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bmi-stats-card.component.html',
  styleUrl: './bmi-stats-card.component.css'
})
export class BmiStatsCardComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  statService: StatisticsService = inject(StatisticsService);
  currentWeight : WeightDto | undefined;
  bmi: Bmi | undefined;
  currentLoss: number = 0;
  constructor() {
  }

  async ngOnInit() {
    this.currentWeight = await this.weightService.getLatestWeight();
    this.bmi = await this.weightService.getLatestBmi();
    this.currentLoss = await this.statService.getCurrentTotalLoss();
  }
}
