import { Component } from '@angular/core';
import {FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-weight-input',
  templateUrl: './weight-input.component.html',
  styleUrls: ['./weight-input.component.css']
})
export class WeightInputComponent {
  numberInput = new FormControl(65.0,[Validators.required, Validators.min(0.0), Validators.max(600.0)]);

  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  saveWeight() {
    console.log(this.numberInput.value);
  }
}
