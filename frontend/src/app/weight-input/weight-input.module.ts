import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeightInputComponent } from './weight-input/weight-input.component';
import {ReactiveFormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    WeightInputComponent
  ],
  exports: [
    WeightInputComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class WeightInputModule { }
