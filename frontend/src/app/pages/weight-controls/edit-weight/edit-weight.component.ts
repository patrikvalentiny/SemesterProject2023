import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {FormControl, Validators} from "@angular/forms";
import {WeightDto} from "../../../dtos/weight-dto";
import {WeightInput} from "../../../dtos/weight-input";

@Component({
    selector: 'app-edit-weight',
    templateUrl: './edit-weight.component.html',
    styleUrls: ['./edit-weight.component.css']
})
export class EditWeightComponent implements OnInit {
    weightService: WeightService = inject(WeightService);

    numberInput: FormControl<number | null> = new FormControl(null, [Validators.required, Validators.min(0.0), Validators.max(600.0)],);
    dateInput:FormControl<string | null> = new FormControl({value:null, disabled:true}, [Validators.required ]);
    // timeInput = new FormControl('', [Validators.required]);
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
    }

    processData(data: WeightDto) {
        this.numberInput.setValue(data.weight);
        this.dateInput.setValue(data.date.toLocaleString().substring(0, 10));
        // this.timeInput.setValue(data.date.toLocaleString().substring(11, 16));

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
        this.weightService.editingWeight.subscribe(i => this.processData(i));
    }
}
