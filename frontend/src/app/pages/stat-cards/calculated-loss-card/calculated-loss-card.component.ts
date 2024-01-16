import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../../services/statistics.service";

@Component({
  selector: 'app-calculated-loss-card',
  standalone: true,
  imports: [],
  templateUrl: './calculated-loss-card.component.html',
  styleUrl: './calculated-loss-card.component.css'
})
export class CalculatedLossCardComponent implements OnInit{
  calculatedWeeklyLoss: number = 0.0;
  calculatedDailyLoss: number = 0.0;
  private readonly statService: StatisticsService = inject(StatisticsService);

  async ngOnInit() {
    try {
      const calculatedDailyLoss = await this.statService.getCalculatedDailyLoss();
      const calculatedWeeklyLoss = await this.statService.getCalculatedWeeklyLoss();
      this.calculatedDailyLoss = calculatedDailyLoss!;
      this.calculatedWeeklyLoss = calculatedWeeklyLoss!;
    } catch (e) {
      //caught by interceptor
      return;
    }
  }
}
