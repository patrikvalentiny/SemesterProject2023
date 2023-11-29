import {Component, inject} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {WeightService} from "../../weight-controls/weight.service";

@Component({
  selector: 'app-onboarding-weight',
  templateUrl: './onboarding-weight.component.html',
  styleUrl: './onboarding-weight.component.css'
})
export class OnboardingWeightComponent {
  weightService: WeightService = inject(WeightService);
  numberInput: FormControl<number | null> = new FormControl(65.0, [Validators.required, Validators.min(25.0), Validators.max(600.0)]);


  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    await this.weightService.postWeight(this.numberInput.value!, new Date().toISOString().substring(0, 10));
  }
}
