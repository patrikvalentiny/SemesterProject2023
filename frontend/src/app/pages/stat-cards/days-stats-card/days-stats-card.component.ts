import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";

@Component({
    selector: 'app-days-stats-card',
    templateUrl: './days-stats-card.component.html',
    styleUrl: './days-stats-card.component.css',
    standalone: true
})
export class DaysStatsCardComponent implements OnInit {
  statService: StatisticsService = inject(StatisticsService);
  daysToGo: number | undefined;
  dayIn: number | undefined;

  constructor() {
  }

  async ngOnInit() {
    try {
      this.daysToGo = await this.statService.getDaysToGo();
      this.dayIn = await this.statService.getDayIn();
    } catch (e) {
      //caught by interceptor
      return;
    }

  }
}
