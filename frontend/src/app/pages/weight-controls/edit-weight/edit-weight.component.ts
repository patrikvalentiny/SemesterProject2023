import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {WeightDto} from "../../../dtos/weight-dto";
import {WeightInput} from "../../../dtos/weight-input";

@Component({
  selector: 'app-edit-weight',
  templateUrl: './edit-weight.component.html',
  styleUrls: ['./edit-weight.component.css']
})
export class EditWeightComponent implements OnInit {
  numberInput: FormControl<number | null> = new FormControl(0.0, [Validators.required, Validators.min(20.0), Validators.max(600.0)]);
  dateInput: FormControl<string | null> = new FormControl({value: "", disabled: true}, [Validators.required]);
  editingWeight: WeightDto | null = null;
  formGroup = new FormGroup({
    numberInput: this.numberInput,
    dateInput: this.dateInput,
    // timeInput: this.timeInput
  })
  // timeInput = new FormControl('', [Validators.required]);
  private readonly weightService: WeightService = inject(WeightService);

  constructor() {
  }

  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    const weight: WeightInput = {
      weight: this.numberInput.value!,
      date: new Date(this.dateInput.value!)
    }
    await this.weightService.putWeight(weight);
    this.editingWeight = weight;
  }

  processData(data: WeightDto) {
    this.numberInput.setValue(data.weight);
    this.dateInput.setValue(data.date.toLocaleString().substring(0, 10));
    // this.timeInput.setValue(data.date.toLocaleString().substring(11, 16));
    this.editingWeight = data;

    return data;
  }

  async deleteWeight() {
    const weight: WeightInput = {
      weight: this.numberInput.value!,
      date: new Date(this.dateInput.value!)
    }
    await this.weightService.deleteWeight(weight);
  }

  async ngOnInit() {
    this.weightService.editingWeight.subscribe(i => this.processData(i!));
  }
}
