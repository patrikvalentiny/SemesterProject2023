import {Component, inject} from '@angular/core';
import {UserDetailsService} from "../../../services/user-details.service";
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from "@angular/forms";
import {HotToastService} from "@ngneat/hot-toast";

@Component({
    host: { class: 'h-full' },
    selector: 'app-onboarding',
    templateUrl: './onboarding.component.html',
    styleUrl: './onboarding.component.css',
    standalone: true,
    imports: [ReactiveFormsModule]
})
export class OnboardingComponent {
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
  private readonly userService: UserDetailsService = inject(UserDetailsService);
  private readonly toast = inject(HotToastService);

  constructor() {
  }

  async createDetails() {
    try {
      await this.userService.createProfile(this.formGroup)
      this.toast.success("Profile created")
    } catch (e) {
      //caught by interceptor
      return;
    }
  }
}
