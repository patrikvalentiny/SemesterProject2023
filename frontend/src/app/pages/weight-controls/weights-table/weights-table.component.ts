import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {WeightInput} from "../../../dtos/weight-input";
import {WeightDto} from "../../../dtos/weight-dto";

@Component({
    selector: 'app-weights-table',
    templateUrl: './weights-table.component.html',
    styleUrls: ['./weights-table.component.css']
})
export class WeightsTableComponent implements OnInit {
    public readonly weightService = inject(WeightService);
    public selectedDate: Date | null = null;
    daysPad: null[] = [];
    days: string[] = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];


    constructor() {
    }

    async ngOnInit() {
        let weights = await this.weightService.getWeights();
        this.setDayPad(weights![0]);
    }

    async deleteWeight(weight: WeightInput) {
        await this.weightService.deleteWeight(weight);
    }

    setEditingWeight(date: Date) {
        this.selectedDate = date;
        this.weightService.setEditingWeight(date);
    }

    setDayPad(weight: WeightDto | undefined) {
        if (!weight) {
            return;
        }
        const date = new Date(weight.date);
        // get day number starting monday
        const day = date.getDay() === 0 ? 6 : date.getDay() - 1;
        this.daysPad = new Array(day);
    }
}
