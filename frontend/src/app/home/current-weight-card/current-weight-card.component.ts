import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../weight-controls/weight.service";
import {UserDetailsService} from "../../user-details/user-details.service";

@Component({
  selector: 'app-current-weight-card',
  templateUrl: './current-weight-card.component.html',
  styleUrl: './current-weight-card.component.css'
})
export class CurrentWeightCardComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  userService: UserDetailsService = inject(UserDetailsService);
  latestWeight : number = 0;
  bmi: number = 0;
  constructor() {
  }

  async ngOnInit() {
    this.latestWeight = await this.weightService.getLatestWeight().then(i => i!.weight);
    await this.userService.getProfile();
    console.log(this.latestWeight)
    const height = this.userService.user!.height / 100;
    this.bmi = this.latestWeight / (height * height);
  }
}
