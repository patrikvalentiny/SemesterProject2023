import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeightInputComponent } from './weight-input/weight-input.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { WeightsTableComponent } from './weights-table/weights-table.component';
import {EditModalComponent} from "./edit-modal/edit-modal.component";
import { EditWeightComponent } from './edit-weight/edit-weight.component';


@NgModule({
    declarations: [
        WeightInputComponent,
        WeightsTableComponent,
        EditModalComponent,
        EditWeightComponent
    ],
    exports: [
        WeightInputComponent,
        WeightsTableComponent
    ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
  ]
})
export class WeightControlsModule { }
