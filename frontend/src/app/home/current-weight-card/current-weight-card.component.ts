import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../weight-controls/weight.service";
import {UserDetailsService} from "../../user-details/user-details.service";
import {Bmi} from "../../bmi";
import {WeightDto} from "../../weight-controls/weight-dto";
import {StatisticsService} from "../../statistics.service";

@Component({
  selector: 'app-current-weight-card',
  templateUrl: './current-weight-card.component.html',
  styleUrl: './current-weight-card.component.css'
})
export class CurrentWeightCardComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  userService: UserDetailsService = inject(UserDetailsService);
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
