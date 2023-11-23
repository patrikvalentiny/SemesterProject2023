import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../weight.service";
import {FormControl, Validators} from "@angular/forms";
import {WeightDto} from "../weight-dto";

@Component({
    selector: 'app-edit-weight',
    templateUrl: './edit-weight.component.html',
    styleUrls: ['./edit-weight.component.css']
})
export class EditWeightComponent implements OnInit {
    weightService: WeightService = inject(WeightService);


    weightId = 0;
    numberInput = new FormControl(0, [Validators.required, Validators.min(0.0), Validators.max(600.0)],);
    dateInput = new FormControl('', [Validators.required]);

    // timeInput = new FormControl('', [Validators.required]);

    constructor() {
        this.weightService.editingWeight.subscribe(i => this.processData(i));
    }


    decrement() {
        this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
    }

    increment() {
        this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
    }

    async saveWeight() {
        await this.weightService.putWeight(this.weightId, this.numberInput.value!, this.dateInput.value!);
    }

    processData(data: WeightDto) {
        this.weightId = data.id;
        this.numberInput.setValue(data.weight);
        this.dateInput.setValue(data.date.toLocaleString().substring(0, 10));
        // this.timeInput.setValue(data.date.toLocaleString().substring(11, 16));

        return data;
    }

    async ngOnInit() {

    }
}
