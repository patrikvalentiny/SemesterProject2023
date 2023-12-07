import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {WeightDto} from "../../../dtos/weight-dto";

@Component({
    selector: 'app-edit-weight',
    templateUrl: './edit-weight.component.html',
    styleUrls: ['./edit-weight.component.css']
})
export class EditWeightComponent implements OnInit {
    weightService: WeightService = inject(WeightService);

    numberInput: FormControl<number | null> = new FormControl(65, [Validators.required, Validators.min(0.0), Validators.max(600.0)],);
    dateInput:FormControl<string | null> = new FormControl({value:null, disabled:true}, [Validators.required ]);
    // timeInput = new FormControl('', [Validators.required]);

    formGroup = new FormGroup({
        numberInput: this.numberInput,
        dateInput: this.dateInput,
        // timeInput: this.timeInput
    })
    constructor() {

    }


    decrement() {
        this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
    }

    increment() {
        this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
    }

    async saveWeight() {
        await this.weightService.putWeight(this.formGroup.value as WeightDto);
    }

    processData(data: WeightDto) {
        this.numberInput.setValue(data.weight);
        this.dateInput.setValue(data.date.toLocaleString().substring(0, 10));
        // this.timeInput.setValue(data.date.toLocaleString().substring(11, 16));

        return data;
    }

    async ngOnInit() {
        this.weightService.editingWeight.subscribe(i => this.processData(i));
    }
}
