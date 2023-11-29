import {Component, inject, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {UserDetailsService} from "../user-details.service";
import {addWarning} from "@angular-devkit/build-angular/src/utils/webpack-diagnostics";
import {FormBuilder, FormControl, ReactiveFormsModule, Validators} from "@angular/forms";
import {UserDetails} from "../user-details";

@Component({
  selector: 'app-account-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './account-details.component.html',
  styleUrl: './account-details.component.css'
})
export class AccountDetailsComponent implements OnInit {
  userService = inject(UserDetailsService);
  heightInput:FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetWeightInput:FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0)]);
  targetDateInput: FormControl<string | null> = new FormControl(null);
  constructor() { }

  async ngOnInit() {
    const user = await this.userService.getProfile()
    if (user != null) {
      this.updateData(user)
    }
  }

  updateData(userDetails: UserDetails) {
    this.heightInput.setValue(userDetails.height);
    this.targetWeightInput.setValue(userDetails.targetWeight);
    console.log(userDetails.targetDate.toString())
    this.targetDateInput.setValue(userDetails.targetDate.toString().substring(0, 10));
  }

  async updateDetails() {
    await this.userService.updateProfile(this.heightInput.value!, this.targetWeightInput.value!, this.targetDateInput.value!)
  }

  async createDetails() {
    await this.userService.createProfile(this.heightInput.value!, this.targetWeightInput.value!, this.targetDateInput.value!)
  }
}
