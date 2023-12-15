import {Component, inject, OnInit} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {WeightService} from "../../../services/weight.service";
import {WeightInput} from "../../../dtos/weight-input";
import {Router} from "@angular/router";

@Component({
  selector: 'app-weight-input',
  templateUrl: './weight-input.component.html',
  styleUrls: ['./weight-input.component.css']
})
export class WeightInputComponent implements OnInit {
  numberInput: FormControl<number | null> = new FormControl(0, [Validators.required, Validators.min(0.0), Validators.max(600.0)]);
  dateInput = new FormControl(new Date().toISOString().substring(0, 10), [Validators.required]);
  dayBeforeWeight: number | undefined;
  private readonly weightService: WeightService = inject(WeightService);
  private readonly router: Router = inject(Router);

  // timeInput = new FormControl(new Date().toISOString().substring(11, 16), [Validators.required]);

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
        date: new Date(this.dateInput.value!).getTimezoneOffset() === 0 ? new Date(this.dateInput.value!) : new Date(this.dateInput.value! + "T00:00:00Z")
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
      const weights = await this.weightService.getWeights();
      if (weights?.length === 0 || weights === undefined) return;
      this.dayBeforeWeight = weights.at(-1)!.weight;
      this.numberInput.setValue(this.dayBeforeWeight);
    } catch (e) {
      //caught by interceptor
      return;
    }

  }
}
