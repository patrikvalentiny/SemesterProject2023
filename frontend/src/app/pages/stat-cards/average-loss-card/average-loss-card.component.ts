import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";

@Component({
    selector: 'app-average-loss-card',
    templateUrl: './average-loss-card.component.html',
    styleUrl: './average-loss-card.component.css',
    standalone: true
})
export class AverageLossCardComponent implements OnInit {
  averageWeeklyLoss: number = 0.0;
  averageDailyLoss: number = 0.0;
  private readonly statService: StatisticsService = inject(StatisticsService);

  async ngOnInit() {
    try {
      const averageLoss = await this.statService.getAverageDailyLoss();
      const averageLossWeekly = await this.statService.getAverageWeeklyLoss();
      this.averageDailyLoss = averageLoss!;
      this.averageWeeklyLoss = averageLossWeekly!;
    } catch (e) {
      //caught by interceptor
      return;
    }

  }
}
