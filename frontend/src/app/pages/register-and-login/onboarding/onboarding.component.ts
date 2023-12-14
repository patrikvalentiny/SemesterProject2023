import {Component, inject} from '@angular/core';
import {UserDetailsService} from "../../../services/user-details.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  host: {class: 'h-full'},
  selector: 'app-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.css'
})
export class OnboardingComponent {
  userService = inject(UserDetailsService);
  heightInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetWeightInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetDateInput: FormControl<string | null> = new FormControl(null, [Validators.required]);
  firstName: FormControl<string | null> = new FormControl(null);
  lastName: FormControl<string | null> = new FormControl(null);
  lossPerWeek: FormControl<number | null> = new FormControl(null);

  formGroup = new FormGroup({
    height: this.heightInput,
    targetWeight: this.targetWeightInput,
    targetDate: this.targetDateInput,
    firstName: this.firstName,
    lastName: this.lastName,
    lossPerWeek: this.lossPerWeek
  })

  constructor() {
  }

  async createDetails() {
    await this.userService.createProfile(this.formGroup)
  }
}
