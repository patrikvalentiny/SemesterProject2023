import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {WeightInputComponent} from './weight-input/weight-input.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {WeightsTableComponent} from './weights-table/weights-table.component';
import {EditWeightComponent} from './edit-weight/edit-weight.component';
import {WeightLineChartComponent} from "../charts/weight-line-chart/weight-line-chart.component";


@NgModule({
    declarations: [
        WeightInputComponent,
        WeightsTableComponent,
        EditWeightComponent
    ],
    exports: [
        WeightInputComponent,
        WeightsTableComponent,
        EditWeightComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        WeightLineChartComponent,
    ]
})
export class WeightControlsModule {
}
