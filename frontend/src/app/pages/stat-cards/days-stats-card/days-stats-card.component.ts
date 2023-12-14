import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";

@Component({
  selector: 'app-days-stats-card',
  templateUrl: './days-stats-card.component.html',
  styleUrl: './days-stats-card.component.css'
})
export class DaysStatsCardComponent implements OnInit {
  statService: StatisticsService = inject(StatisticsService);
  daysToGo: number | undefined;
  dayIn: number | undefined;

  constructor() {
  }

  async ngOnInit() {
    this.daysToGo = await this.statService.getDaysToGo();
    this.dayIn = await this.statService.getDayIn();
  }
}
