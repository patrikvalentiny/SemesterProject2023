import {Component, inject} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {WeightService} from "../../weight-controls/weight.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-onboarding-weight',
  templateUrl: './onboarding-weight.component.html',
  styleUrl: './onboarding-weight.component.css'
})
export class OnboardingWeightComponent {
  weightService: WeightService = inject(WeightService);
  router = inject(Router);
  numberInput: FormControl<number | null> = new FormControl(65.0, [Validators.required, Validators.min(25.0), Validators.max(600.0)]);


  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    try {
      await this.weightService.postWeight(this.numberInput.value!, new Date().toISOString().substring(0, 10));
      await this.router.navigate(["../onboarding/profile"])
    } catch (e) {

    }

  }
}
