import {Component, inject} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {WeightService} from "../weight.service";

@Component({
  selector: 'app-weight-input',
  templateUrl: './weight-input.component.html',
  styleUrls: ['./weight-input.component.css']
})
export class WeightInputComponent {
  weightService: WeightService = inject(WeightService);
  numberInput = new FormControl(65.0,[Validators.required, Validators.min(0.0), Validators.max(600.0)]);
  dateInput = new FormControl(new Date().toISOString().substring(0,10),[Validators.required]);

  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    await this.weightService.postWeight(this.numberInput.value!, new Date(this.dateInput.value!));
  }
}
