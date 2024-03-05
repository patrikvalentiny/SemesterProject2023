import {Component, inject, OnInit} from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from "@angular/forms";
import {WeightService} from "../../../services/weight.service";
import {WeightInput} from "../../../dtos/weight-input";
import {Router} from "@angular/router";
import { WeightLineChartComponent } from '../../../charts/weight-line-chart/weight-line-chart.component';
import {WeightDto} from "../../../dtos/weight-dto";

@Component({
    selector: 'app-weight-input',
    templateUrl: './weight-input.component.html',
    styleUrls: ['./weight-input.component.css'],
    standalone: true,
    imports: [WeightLineChartComponent, ReactiveFormsModule]
})
export class WeightInputComponent implements OnInit {
  weightInput: FormControl<number | null> = new FormControl(0, [Validators.required, Validators.min(0.0), Validators.max(600.0)]);
  dateInput = new FormControl(new Date().toISOString().substring(0, 10), [Validators.required]);
  dayBeforeWeight: WeightDto | undefined;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly router: Router = inject(Router);

  // timeInput = new FormControl(new Date().toISOString().substring(11, 16), [Validators.required]);
  bodyFatInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0.0), Validators.max(80.0)])
  skeletalMuscleInput: FormControl<number | null>  = new FormControl(null, [Validators.required, Validators.min(0.0), Validators.max(200.0)]);

  decrement() {
    this.weightInput.setValue(Number((this.weightInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.weightInput.setValue(Number((this.weightInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    try {
      const weight: WeightInput = {
        weight: this.weightInput.value!,
        date: new Date(this.dateInput.value!).getTimezoneOffset() === 0 ? new Date(this.dateInput.value!) : new Date(this.dateInput.value! + "T00:00:00Z"),
        bodyFatPercentage: this.bodyFatInput.value ?? null,
        skeletalMuscleWeight: this.skeletalMuscleInput.value ?? null
      }
      await this.weightService.postWeight(weight);
      await this.router.navigate(['../home']);
    } catch (e) {
      //caught by interceptor
      return;
    }

  }

  async ngOnInit() {
    try {
      this.dayBeforeWeight = await this.weightService.getLatestWeight();
      this.weightInput.setValue(this.dayBeforeWeight?.weight ?? 0);
      this.bodyFatInput.setValue(this.dayBeforeWeight?.bodyFatPercentage ?? null);
      this.skeletalMuscleInput.setValue(this.dayBeforeWeight?.skeletalMuscleWeight ?? null);
    } catch (e) {
      //caught by interceptor
      return;
    }

  }
}
