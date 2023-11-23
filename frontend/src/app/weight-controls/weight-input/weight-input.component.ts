import {Component, inject, OnInit} from '@angular/core';
import {FormControl, Validators} from "@angular/forms";
import {WeightService} from "../weight.service";

@Component({
    selector: 'app-weight-input',
    templateUrl: './weight-input.component.html',
    styleUrls: ['./weight-input.component.css']
})
export class WeightInputComponent implements OnInit {
    weightService: WeightService = inject(WeightService);
    numberInput: FormControl<number | null> = new FormControl(0, [Validators.required, Validators.min(0.0), Validators.max(600.0)]);
    dateInput = new FormControl(new Date().toISOString().substring(0, 10), [Validators.required]);

    // timeInput = new FormControl(new Date().toISOString().substring(11, 16), [Validators.required]);


    decrement() {
        this.numberInput.setValue(Number((this.numberInput.value! - 0.1).toFixed(1)))
    }

    increment() {
        this.numberInput.setValue(Number((this.numberInput.value! + 0.1).toFixed(1)))
    }

    async saveWeight() {
        await this.weightService.postWeight(this.numberInput.value!, this.dateInput.value!);
    }

    async ngOnInit() {
        const weightDto = await this.weightService.getLatestWeight();
        this.numberInput.setValue(weightDto?.weight ?? 0.0);
    }
}
