import {Component, inject} from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from "@angular/forms";
import {WeightService} from "../../../services/weight.service";
import {Router} from "@angular/router";
import {WeightInput} from "../../../dtos/weight-input";

@Component({
    selector: 'app-onboarding-weight',
    templateUrl: './onboarding-weight.component.html',
    styleUrl: './onboarding-weight.component.css',
    standalone: true,
    imports: [ReactiveFormsModule]
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
      const weight: WeightInput = {
        weight: this.numberInput.value!,
        date: new Date().getTimezoneOffset() === 0 ? new Date() : new Date(new Date().toISOString().substring(0, 10) + "T00:00:00Z")
      };
      await this.weightService.postWeight(weight);
      await this.router.navigate(["../onboarding/profile"])
    } catch (e) {
      //caught by interceptor
      return;
    }

  }
}
