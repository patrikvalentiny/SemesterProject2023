import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../weight.service";
import {FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-edit-weight',
  templateUrl: './edit-weight.component.html',
  styleUrls: ['./edit-weight.component.css']
})
export class EditWeightComponent implements OnInit {
  weightService: WeightService = inject(WeightService);
  numberInput = new FormControl(this.weightService.editingWeight?.weight ?? 0, [Validators.required, Validators.min(0.0), Validators.max(600.0)]);
  dateInput = new FormControl(this.weightService.editingWeight?.date.substring(0, 10) ?? '', [Validators.required]);
  timeInput = new FormControl(this.weightService.editingWeight?.date.substring(11, 16) ?? '', [Validators.required]);


  decrement() {
    this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
  }

  increment() {
    this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
  }

  async saveWeight() {
    await this.weightService.putWeight(this.weightService.editingWeight!.id, this.numberInput.value!, this.dateInput.value!, this.timeInput.value!);
  }

  async ngOnInit() {

  }
}
