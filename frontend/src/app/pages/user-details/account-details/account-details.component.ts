import {Component, inject, OnInit} from '@angular/core';
import {UserDetailsService} from "../../../services/user-details.service";
import { FormControl, FormGroup, Validators, ReactiveFormsModule, FormsModule } from "@angular/forms";
import {UserDetails} from "../../../dtos/user-details";
import {HotToastService} from "@ngneat/hot-toast";

@Component({
    selector: 'app-account-details',
    templateUrl: './account-details.component.html',
    styleUrl: './account-details.component.css',
    standalone: true,
    imports: [ReactiveFormsModule, FormsModule]
})
export class AccountDetailsComponent implements OnInit {
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
  private readonly userService = inject(UserDetailsService);
  private readonly toast = inject(HotToastService);


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
    try {
      await this.userService.updateProfile(this.formGroup)
      this.toast.success("Profile updated")
    } catch (e) {
      return;
    }

  }

  async deleteAccount(){
    try {
      await this.userService.deleteProfile();
      this.toast.success("Account deleted")
    } catch (e) {
      return;
    }
  };
}
