import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";

@Component({
  selector: 'app-average-loss-card',
  templateUrl: './average-loss-card.component.html',
  styleUrl: './average-loss-card.component.css'
})
export class AverageLossCardComponent  implements OnInit{
  private readonly statService: StatisticsService = inject(StatisticsService);
  averageWeeklyLoss: number = 0.0;
  averageDailyLoss: number = 0.0;

  async ngOnInit() {
    const averageLoss = await this.statService.getAverageDailyLoss();
    const averageLossWeekly = await this.statService.getAverageWeeklyLoss();
    this.averageDailyLoss = averageLoss!;
    this.averageWeeklyLoss = averageLossWeekly!;
  }
}
