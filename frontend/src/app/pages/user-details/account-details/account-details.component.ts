import {Component, inject, OnInit} from '@angular/core';
import {UserDetailsService} from "../../../services/user-details.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserDetails} from "../../../dtos/user-details";

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent implements OnInit {
  userService = inject(UserDetailsService);
  heightInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetWeightInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetDateInput: FormControl<string | null> = new FormControl(null);
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

  async ngOnInit() {
    const user = await this.userService.getProfile()
    if (user != null) {
      this.updateData(user)
    }
  }

  updateData(userDetails: UserDetails) {
    this.firstName.setValue(userDetails.firstname);
    this.lastName.setValue(userDetails.lastname);
    this.heightInput.setValue(userDetails.height);
    this.targetWeightInput.setValue(userDetails.targetWeight);
    this.targetDateInput.setValue(userDetails.targetDate.toString().substring(0, 10));
    this.lossPerWeek.setValue(userDetails.lossPerWeek);
  }

  async updateDetails() {
    await this.userService.updateProfile(this.formGroup)
  }

  async createDetails() {
    await this.userService.createProfile(this.formGroup)
  }
}
